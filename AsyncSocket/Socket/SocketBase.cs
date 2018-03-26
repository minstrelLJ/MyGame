﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace AsyncSocket
{
    public class SocketBase
    {
        protected TcpClient tcpClient;
        protected string host;
        protected int port;

        public SocketAsyncEventArgs ReceiveEventArgs { get; set; }
        public SocketAsyncEventArgs SendEventArgs { get; set; }

        public BufferRecive ReceiveBuffer { get; set; }
        public BufferSend SendBuffer { get; set; }

        public DateTime ConnectDateTime { get; set; }
        public DateTime ActiveTime { get; set; }

        protected byte[] asyncReceiveBuffer;

        public Socket ConnectSocket
        {
            get { return connectSocket; }
            set
            {
                connectSocket = value;

                if (connectSocket == null) //清理缓存
                {
                    if (tcpClient != null)
                        tcpClient.Close();

                    ReceiveBuffer.Clear(ReceiveBuffer.DataCount);
                    SendBuffer.ClearPacket();
                }

                tcpClient.Client = value;
                ReceiveEventArgs.AcceptSocket = connectSocket;
                SendEventArgs.AcceptSocket = connectSocket;
            }
        }
        protected Socket connectSocket;

        public SocketBase()
        {
            tcpClient = new TcpClient();
            tcpClient.Client.Blocking = true; //使用阻塞模式，即同步模式

            int asyncReceiveBufferSize = ProtocolConst.ReceiveBufferSize;

            ReceiveEventArgs = new SocketAsyncEventArgs();
            ReceiveEventArgs.UserToken = this;
            asyncReceiveBuffer = new byte[asyncReceiveBufferSize];
            ReceiveEventArgs.SetBuffer(asyncReceiveBuffer, 0, asyncReceiveBuffer.Length);

            SendEventArgs = new SocketAsyncEventArgs();
            SendEventArgs.UserToken = this;

            ReceiveBuffer = new BufferRecive(asyncReceiveBufferSize);
            SendBuffer = new BufferSend(asyncReceiveBufferSize);
        }

        public void SendMessage(DataBase db)
        {
            lock (SendBuffer)
            {
                SendBuffer.Clear();
                SendBuffer.Write(db);
                tcpClient.Client.Send(SendBuffer.Buffer, 0, SendBuffer.DataCount, SocketFlags.None);
            }
        }
    }
}
