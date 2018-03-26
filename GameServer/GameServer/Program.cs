using System;
using System.IO;
using AsyncSocket;
using Tools;

namespace GameServer
{
    class Program
    {
        public static ILog logger;
        
        static void Main(string[] args)
        {
            Init();

            Console.ReadLine();
        }

        public static void Init()
        {
            Global.InitLogger(LogType.ConsoleWriteLine);
            logger = Global.Logger;

            ConfigManager.Instance.Init();
            NetworkManager.Instance.Init();
            ServerManager.Instance.Init();
            DataManager.Instance.Init();
        }
    }
}
