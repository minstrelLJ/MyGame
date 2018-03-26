using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsyncSocket;

namespace GameServer
{
    public class ServerPool
    {
        private Stack<Server> pool;

        public ServerPool(int capacity)
        {
            pool = new Stack<Server>(capacity);

            Server server;
            for (int i = 0; i < 100; i++)
            {
                server = new Server();
                pool.Push(server);
            }
        }

        public void Push(Server item)
        {
            if (item == null)
                Global.Logger.Error("Items added to a Server cannot be null");

            lock (pool)
            {
                pool.Push(item);
            }
        }

        public Server Pop()
        {
            lock (pool)
            {
                Server server = pool.Pop();
                if (server == null)
                {
                    server = new Server();
                    pool.Push(server);
                }
                return server;
            }
        }

        public int Count
        {
            get { return pool.Count; }
        }
    }
}
