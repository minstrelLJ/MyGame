using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AsyncSocket;

namespace AsyncSocket
{
    public class SocketClient : SocketBase
    {
        public Action<DataBase> Receive;

        public SocketClient()
        {
            ReceiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            SendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
        }

        public bool Connect(string _host, int _port)
        {
            try
            {
                tcpClient.Connect(_host, _port);
                ConnectSocket = tcpClient.Client;
                tcpClient.Client.ReceiveAsync(ReceiveEventArgs);
                host = _host;
                port = _port;
                Global.Logger.Debug("Connect to server {0}:{1}", host, port);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        void IO_Completed(object sender, SocketAsyncEventArgs asyncEventArgs)
        {
            ActiveTime = DateTime.Now;
            try
            {
                if (asyncEventArgs.LastOperation == SocketAsyncOperation.Receive)
                {
                    ReceiveEventArgs = asyncEventArgs;
                    ProcessReceive();
                }
                else if (asyncEventArgs.LastOperation == SocketAsyncOperation.Send)
                {
                    SendEventArgs = asyncEventArgs;
                    ProcessSend();
                }
                else
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
            catch (Exception E)
            {
                Global.Logger.Error("IO_Completed {0} error, message: {1}", ConnectSocket, E.Message);
                Global.Logger.Error(E.StackTrace);
            }
        }
        protected void ProcessReceive()
        {
            if (ConnectSocket == null)
                return;

            ActiveTime = DateTime.Now;
            if (ReceiveEventArgs.BytesTransferred > 0 && ReceiveEventArgs.SocketError == SocketError.Success)
            {
                int offset = ReceiveEventArgs.Offset;
                int count = ReceiveEventArgs.BytesTransferred;

                if (count > 0) //处理接收数据
                {
                    //如果处理数据返回失败，则断开连接
                    if (!ProcessReceive(ReceiveEventArgs.Buffer, offset, count))
                    {
                        Close();
                    }
                    else //否则投递下次介绍数据请求
                    {
                        bool willRaiseEvent = ConnectSocket.ReceiveAsync(ReceiveEventArgs); //投递接收请求
                        if (!willRaiseEvent)
                            ProcessReceive();
                    }
                }
                else
                {
                    bool willRaiseEvent = ConnectSocket.ReceiveAsync(ReceiveEventArgs); //投递接收请求
                    if (!willRaiseEvent)
                        ProcessReceive();
                }
            }
            else
            {
                Close();
            }
        }

        protected bool ProcessSend()
        {
            ActiveTime = DateTime.Now;
            if (SendEventArgs.SocketError == SocketError.Success)
                return SendCompleted(); //调用子类回调函数
            else
            {
                Close();
                return false;
            }
        }

        private bool ProcessReceive(byte[] buffer, int offset, int count) //接收异步事件返回的数据，用于对数据进行缓存和分包
        {
            try
            {
                ActiveTime = DateTime.UtcNow;
                ReceiveBuffer.WriteBuffer(buffer, offset, count);

                int Len = BitConverter.ToInt32(buffer, offset); //取出信息长度
                offset += sizeof(int);

                // 收到的数据不全
                if (Len + sizeof(int) > count)
                    return true;

                string msg = Encoding.UTF8.GetString(buffer, offset, Len);
                ReceiveBuffer.Clear(Len + sizeof(int));

                if (Receive != null)
                    Receive(new ServerData(msg));
                
                return true;
            }
            catch (Exception e)
            {
                Global.Logger.Error(e.Message);
                return false;
            }
        }
        private bool SendCompleted()
        {
            return true;
        }

        public void Close()
        {
            tcpClient.Client.Close();
        }
    }
}