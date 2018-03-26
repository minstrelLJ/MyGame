using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace GameServer
{
    public class NetworkManager : Singleton<NetworkManager>
    {
        ListenServer listenServer;

        public void Init()
        {
            listenServer = new ListenServer();
            listenServer.Start();
        }
    }
}
