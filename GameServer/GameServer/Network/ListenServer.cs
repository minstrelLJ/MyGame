using System;
using System.Configuration;
using AsyncSocket;
using Tools;

namespace GameServer
{
    public class ListenServer
    {
        public SocketServer listenSocket;

        public void Start()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            int port = 0;
            if (!(int.TryParse(config.AppSettings.Settings["Port"].Value, out port)))
                port = 9999;

            int parallelNum = 0;
            if (!(int.TryParse(config.AppSettings.Settings["ParallelNum"].Value, out parallelNum)))
                parallelNum = 8000;

            int socketTimeOutMS = 0;
            if (!(int.TryParse(config.AppSettings.Settings["SocketTimeOutMS"].Value, out socketTimeOutMS)))
                socketTimeOutMS = 5 * 60 * 1000;

            listenSocket = new SocketServer(port, parallelNum, socketTimeOutMS);
            listenSocket.Start();
            listenSocket.Receive = (client, data) => { ParseCMD.Parse(client, data); };
        }
    }
}
