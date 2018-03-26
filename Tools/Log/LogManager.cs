using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    public class LogManager
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
        /// 获取日志实体
        /// </summary>
        /// <returns></returns>
        public static ILog GetLogger(LogType type)
        {
            return new Log(type);
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
