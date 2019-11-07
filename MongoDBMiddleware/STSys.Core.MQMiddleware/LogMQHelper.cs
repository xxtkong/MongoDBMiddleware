using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace STSys.Core.MQMiddleware
{
    public class LogMQHelper
    {
        public delegate NotificationEventArgs BeforeEnQueueEventHandler(QueueInfo target);
        private BeforeEnQueueEventHandler myevent;
        public event BeforeEnQueueEventHandler BeforeEnQueueEvent
        {
            add
            {
                if (myevent == null)
                    myevent += value;
            }
            remove
            {
                myevent -= value;
            }
        }
        public event EventHandler<NotificationEventArgs> NotificationEventArgs;
        #region  
        public readonly static LogMQHelper Instance = new LogMQHelper();
        private LogMQHelper()
        { }
        private Queue<QueueInfo> ListQueue = new Queue<QueueInfo>();
        /// <summary>
        /// 入队
        /// </summary>
        public void AddQueue(string level,string name, string msg)
        {
            var queueinfo = new QueueInfo() { level = level, name = name, msg = msg };
            ListQueue.Enqueue(queueinfo);
        }
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            Thread thread = new Thread(threadStart);
            thread.IsBackground = true;
            thread.Start();
        }
        private void threadStart()
        {
            while (true)
            {
                if (ListQueue.Count > 0)
                {
                    try
                    {
                        ScanQueue();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                    Thread.Sleep(3000);
            }
        }
        private static object obj = new object();
        /// <summary>
        /// 出对
        /// </summary>
        private void ScanQueue()
        {
            while (ListQueue.Count > 0)
            {
                try
                {
                    lock (obj)
                    {
                        var queueinfo = ListQueue.Dequeue();
                        if (this.myevent != null)
                        {
                            var result = this.myevent(queueinfo);
                            //写入MongoDb
                            //NotificationEventArgs(this, result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    NotificationEventArgs(this, new NotificationEventArgs() { Ok = false, Msg = "" });
                }
            }
        }
        #endregion
    }
}
