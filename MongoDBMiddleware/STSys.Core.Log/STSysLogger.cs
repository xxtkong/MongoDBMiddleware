using Microsoft.Extensions.Logging;
using Repository;
using STSys.Core.MQMiddleware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace STSys.Core.Log
{
    public class STSysLogger : ILogger
    {
        private string _name;
        private STSysLoggerConfiguration _config;
        private readonly IRepositoryMongoDB<QueueInfo> _repositoryMongo;
        public STSysLogger(string name, STSysLoggerConfiguration config)
        {
            _name = name;
            _config = config;
            _repositoryMongo = config.repositoryMongoDB;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;
            if (logLevel.ToString().Equals("Error"))
            {
                var helper = LogMQHelper.Instance;
                helper.BeforeEnQueueEvent += Helper_BeforeEnQueueEvent;
                helper.AddQueue(logLevel.ToString(), _name, formatter(state, exception));
            }
            if (logLevel.ToString().Equals("Debug"))
            {
                WriteLogFile(logLevel.ToString(), _name, formatter(state, exception));
            }
        }

        private NotificationEventArgs Helper_BeforeEnQueueEvent(QueueInfo target)
        {
            _repositoryMongo.Add(target);
            return new NotificationEventArgs() { Ok = true, Msg = "" };
        }

        public static string path = "c:\\logs";
        /// <summary>
        /// 直接写文件，文件地址D:\\logs
        /// </summary>
        /// <param name="type"></param>
        /// <param name="className"></param>
        /// <param name="content"></param>
        public static void WriteLogFile(string type, string className, string content)
        {
            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名
            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter mySw = File.AppendText(filename);
            //向日志文件写入内容
            string write_content = time + " " + type + " " + className + ": " + content;
            mySw.WriteLine(write_content);
            //关闭日志文件
            mySw.Close();
        }
    }
}
