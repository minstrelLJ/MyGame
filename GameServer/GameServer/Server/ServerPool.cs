using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsyncSocket;

namespace GameServer
{
    public class ServerPool
    {
        private Stack<BattleServer> pool;

        public ServerPool(int capacity)
        {
            pool = new Stack<BattleServer>(capacity);

            BattleServer server;
            for (int i = 0; i < 100; i++)
            {
                server = new BattleServer();
                pool.Push(server);
            }
        }

        public void Push(BattleServer item)
        {
            if (item == null)
                Global.Logger.Error("Items added to a Server cannot be null");

            lock (pool)
            {
                pool.Push(item);
            }
        }

        public BattleServer Pop()
        {
            lock (pool)
            {
                BattleServer server = pool.Pop();
                if (server == null)
                {
                    server = new BattleServer();
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
