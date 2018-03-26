using System;
using AsyncSocket;
using Tools;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketClient Socket = new SocketClient();
            Socket.Connect("127.0.0.1", 9999);
            Socket.Receive = (data) => 
            {
                Console.WriteLine(data.cmd);
            };

            DataBase db = DataPool.Instance.Pop(CMD.Heartbeat);
            db.Add("你好");
            Socket.SendMessage(db);

            Console.ReadLine();
        }
    }
}
