using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    /// <summary>
    /// 日志
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 一般日志
        /// </summary>
        /// <param name="format">信息</param>
        /// <param name="args">参数</param>
        void Debug(string format, params object[] args);

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="format">信息</param>
        /// <param name="args">参数</param>
        void Warning(string format, params object[] args);

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="format">信息</param>
        /// <param name="args">参数</param>
        void Error(string format, params object[] args);
    }
}
