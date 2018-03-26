using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace AsyncSocket
{
    public class Global
    {
        public static ILog Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = LogManager.GetLogger(LogType.ConsoleWriteLine);
                }
                return logger;
            }
        }
        private static ILog logger;

        /// <summary>
        /// 初始化日志管理类
        /// </summary>
        /// <param name="type">日志类型</param>
        public static void InitLogger(LogType type)
        {
            logger = LogManager.GetLogger(type);
        }
    }

    public enum CMD
    {
        None = 0,
        Heartbeat = 1,
        Login = 1000,                   // 登陆
        Register = 1001,               // 注册
        EnterGame = 1002,           // 进入游戏
        GetRole = 1003,                // 获取角色信息
    }
}
