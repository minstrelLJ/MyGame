using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Net;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace AsyncSocket
{
    public class SocketServer
    {
        public Action<AsyncSocketUserToken, ClientData> Receive;
        private Socket socketServer;
        private int port;
        private int parallelNum;
        private int socketTimeOutMS; //Socket最大超时时间，单位为MS
        private int receiveBufferSize; //每个连接接收缓存大小
        private Semaphore maxNumberAcceptedClients; //限制访问接收连接的线程数，用来控制最大并发数

        private AsyncSocketUserTokenList asyncSocketUserTokenList;
        private AsyncSocketUserTokenPool asyncSocketUserTokenPool;

        /// <summary>
        /// 初始化服务器
        /// </summary>
        /// <param name="_port">端口</param>
        /// <param name="_parallelNum">最大连接数</param>
        /// <param name="_socketTimeOutMS">超时，单位毫秒</param>
        public SocketServer(int _port, int _parallelNum, int _socketTimeOutMS)
        {
            port = _port;
            parallelNum = _parallelNum;
            socketTimeOutMS = _socketTimeOutMS;

            receiveBufferSize = ProtocolConst.ReceiveBufferSize;
            maxNumberAcceptedClients = new Semaphore(parallelNum, parallelNum);
            asyncSocketUserTokenList = new AsyncSocketUserTokenList();
            asyncSocketUserTokenPool = new AsyncSocketUserTokenPool(parallelNum);

            AsyncSocketUserToken userToken;
            for (int i = 0; i < parallelNum; i++) //按照连接数建立读写对象
            {
                userToken = new AsyncSocketUserToken();
                userToken.ReceiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                userToken.SendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                asyncSocketUserTokenPool.Push(userToken);
            }
        }

        public void Start()
        {
            IPEndPoint listenPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
            socketServer = new Socket(listenPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socketServer.Bind(listenPoint);
            socketServer.Listen(parallelNum);

            Console.WriteLine("Start listen socket {0} success", listenPoint.ToString());
            Global.Logger.Debug("Start listen socket {0} success", listenPoint.ToString());
            StartAccept(null);
        }
        private void StartAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs == null)
            {
                acceptEventArgs = new SocketAsyncEventArgs();
                acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                acceptEventArgs.AcceptSocket = null; //释放上次绑定的Socket，等待下一个Socket连接
            }

            maxNumberAcceptedClients.WaitOne(); //获取信号量
            bool willRaiseEvent = socketServer.AcceptAsync(acceptEventArgs);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArgs);
            }
        }
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs acceptEventArgs)
        {
            try
            {
                ProcessAccept(acceptEventArgs);
            }
            catch (Exception E)
            {
                Global.Logger.Error("Accept client {0} error, message: {1}", acceptEventArgs.AcceptSocket, E.Message);
                Global.Logger.Error(E.StackTrace);
            }
        }
        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            AsyncSocketUserToken userToken = asyncSocketUserTokenPool.Pop();
            asyncSocketUserTokenList.Add(userToken); //添加到正在连接列表
            userToken.ConnectSocket = acceptEventArgs.AcceptSocket;
            userToken.ConnectDateTime = DateTime.Now;
            userToken.serverSocket = this;
            Global.Logger.Debug("New client {0} accept. Client count {1}", acceptEventArgs.AcceptSocket.RemoteEndPoint, asyncSocketUserTokenList.Count);

            try
            {
                bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                if (!willRaiseEvent)
                {
                    lock (userToken)
                    {
                        ProcessReceive(userToken.ReceiveEventArgs);
                    }
                }
            }
            catch (Exception E)
            {
                Global.Logger.Error("Accept client {0} error, message: {1}", userToken.ConnectSocket, E.Message);
                Global.Logger.Error(E.StackTrace);
            }

            StartAccept(acceptEventArgs); //把当前异步事件释放，等待下次连接
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs asyncEventArgs)
        {
            AsyncSocketUserToken userToken = asyncEventArgs.UserToken as AsyncSocketUserToken;
            userToken.ActiveTime = DateTime.Now;
            try
            {
                lock (userToken)
                {
                    if (asyncEventArgs.LastOperation == SocketAsyncOperation.Receive)
                        ProcessReceive(asyncEventArgs);
                    else if (asyncEventArgs.LastOperation == SocketAsyncOperation.Send)
                        ProcessSend(asyncEventArgs);
                    else
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (Exception E)
            {
                Global.Logger.Error("IO_Completed {0} error, message: {1}", userToken.ConnectSocket, E.Message);
                Global.Logger.Error(E.StackTrace);
            }
        }
        private void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            AsyncSocketUserToken userToken = receiveEventArgs.UserToken as AsyncSocketUserToken;
            if (userToken.ConnectSocket == null)
                return;
            userToken.ActiveTime = DateTime.Now;
            if (userToken.ReceiveEventArgs.BytesTransferred > 0 && userToken.ReceiveEventArgs.SocketError == SocketError.Success)
            {
                int offset = userToken.ReceiveEventArgs.Offset;
                int count = userToken.ReceiveEventArgs.BytesTransferred;

                if (count > 0) //处理接收数据
                {
                    //如果处理数据返回失败，则断开连接
                    if (!userToken.ProcessReceive(userToken.ReceiveEventArgs.Buffer, offset, count))
                    {
                        CloseClientSocket(userToken);
                    }
                    else //否则投递下次介绍数据请求
                    {
                        bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                        if (!willRaiseEvent)
                            ProcessReceive(userToken.ReceiveEventArgs);
                    }
                }
                else
                {
                    bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                    if (!willRaiseEvent)
                        ProcessReceive(userToken.ReceiveEventArgs);
                }
            }
            else
            {
                CloseClientSocket(userToken);
            }
        }
        private bool ProcessSend(SocketAsyncEventArgs sendEventArgs)
        {
            AsyncSocketUserToken userToken = sendEventArgs.UserToken as AsyncSocketUserToken;
            userToken.ActiveTime = DateTime.Now;
            if (sendEventArgs.SocketError == SocketError.Success)
                return userToken.SendCompleted(); //调用子类回调函数
            else
            {
                CloseClientSocket(userToken);
                return false;
            }
        }

        private void CloseClientSocket(AsyncSocketUserToken userToken)
        {
            if (userToken.ConnectSocket == null)
                return;

            string socketInfo = string.Format("Remote Address: {0}", userToken.ConnectSocket.RemoteEndPoint);
            socketInfo = string.Format("Client disconnected {0}.", socketInfo);

            try
            {
                userToken.ConnectSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception E)
            {
                Global.Logger.Error("CloseClientSocket Disconnect client {0} error, message: {1}", socketInfo, E.Message);
            }

            userToken.ConnectSocket.Close();
            userToken.ConnectSocket = null; //释放引用，并清理缓存，包括释放协议对象等资源

            maxNumberAcceptedClients.Release();
            asyncSocketUserTokenPool.Push(userToken);
            asyncSocketUserTokenList.Remove(userToken);

            Global.Logger.Debug("{0} Client count {1}", socketInfo, asyncSocketUserTokenList.Count);
        }
    }
}
