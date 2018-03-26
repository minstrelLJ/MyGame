using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tools;

namespace Tools
{
    public delegate void ShowLog(string format, params object[] args);

    class Log : ILog
    {
        public ShowLog EventDebug;
        public ShowLog EventWarning;
        public ShowLog EventError;

        private LogType logType { get; set; }

        public Log(LogType type)
        {
            logType = type;
        }
        public void SetLogType(LogType type)
        {
            logType = type;
        }

        public void Debug(string format, params object[] args)
        {
            switch (logType)
            {
                case LogType.ConsoleWriteLine: CWDebug(format, args); break;
                case LogType.Text: Set2TextDebug(format, args); break;
                case LogType.Custom: EventDebug(format, args); break;
            }
        }
        public void Warning(string format, params object[] args)
        {
            switch (logType)
            {
                case LogType.ConsoleWriteLine: CWWarning(format, args); break;
                case LogType.Text: Set2TextWarning(format, args); break;
                case LogType.Custom: EventWarning(format, args); break;
            }
        }
        public void Error(string format, params object[] args)
        {
            switch (logType)
            {
                case LogType.ConsoleWriteLine: CWError(format, args); break;
                case LogType.Text: Set2TextError(format, args); break;
                case LogType.Custom: EventError(format, args); break;
            }
        }

        private void ConsoleWriteLine(LogLevel level, string format, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug: if (Console.ForegroundColor != ConsoleColor.Gray) Console.ForegroundColor = ConsoleColor.Gray; break;
                case LogLevel.Warning:if (Console.ForegroundColor != ConsoleColor.Yellow) Console.ForegroundColor = ConsoleColor.Yellow;  break;
                case LogLevel.Error:if (Console.ForegroundColor != ConsoleColor.Red) Console.ForegroundColor = ConsoleColor.Red; break;
            }

            string msg = string.Format(format, args);
            Console.WriteLine(msg);
        }
        private void CWDebug(string format, params object[] args)
        {
            ConsoleWriteLine(LogLevel.Debug, format, args);
        }
        private void CWWarning(string format, params object[] args)
        {
            ConsoleWriteLine(LogLevel.Warning, format, args);
        }
        private void CWError(string format, params object[] args)
        {
            ConsoleWriteLine(LogLevel.Error, format, args);
        }
        
        private void Set2Text(LogLevel level, string format, params object[] args)
        {
            string msg = string.Format(format, args);

            string FileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(FileDirectory))
                Directory.CreateDirectory(FileDirectory);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            FileIO.AddText(FileDirectory + "/" + fileName, DateTime.Now.ToString("HH:mm:ss ") + msg);
        }
        private void Set2TextDebug(string format, params object[] args)
        {
            Set2Text(LogLevel.Debug, "[Debug]:" + format, args);
        }
        private void Set2TextWarning(string format, params object[] args)
        {
            Set2Text(LogLevel.Warning, "[Warning]:" + format, args);
        }
        private void Set2TextError(string format, params object[] args)
        {
            Set2Text(LogLevel.Error, "[Error]:" + format, args);
        }
    }
}
