using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    public class LogManager : Singleton<LogManager>
    {
        public ILog Logger 
        {
            get 
            {
                if (logger == null)
                    logger = new Log(LogType.ConsoleWriteLine);
                
                return logger;
            }
            set { logger = value; }
        }
        private ILog logger;

        /// <summary>
        /// 设置日志类型
        /// </summary>
        /// <param name="type">类型</param>
        public void SetLogType(LogType type)
        {
            Logger.SetLogType(type);
        }
    }

    public enum LogType
    {
        ConsoleWriteLine,
        Text,
        Custom,
    }

    public enum LogLevel
    {
        Debug,
        Warning,
        Error
    }
}
