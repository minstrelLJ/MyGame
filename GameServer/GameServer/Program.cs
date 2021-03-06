﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AsyncSocket;
using Tools;

namespace GameServer
{
    class Program
    {
        public static ILog logger;
        
        static void Main(string[] args)
        {
            //MySqlTemplate.CreateSetting();
            //MySqlTemplate.CreateUser();
            //MySqlTemplate.CreateRole();

            Init();


            //MySqlTemplate.AddUser();

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
