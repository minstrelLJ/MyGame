using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace GameServer
{
    public class ServerManager : Singleton<ServerManager>
    {
        private ServerPool serverPool;

        public void Init()
        {
            serverPool = new ServerPool(100);
        }
    }
}
