﻿using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STSys.Core.MQ
{ 
    public class BusBuilder
    {
        private string _connectionString { get; set; }
        private string _connectionConfigName { get; set; }
        public BusBuilder(string host)
        {
            _connectionConfigName = $"rabbitmq-{host}";
            _connectionString = ConfigManagerConf.GetValue("RabbitMQ:Connection") ?? "";
        }
        public IBus CreateMessageBus()
        {
            // 消息服务器连接字符串
            if (_connectionString == null || _connectionString == string.Empty) throw new Exception($"config[{_connectionConfigName}] not exist,  messageserver connection string is missing or empty");
            return RabbitHutch.CreateBus(_connectionString);
        }
    }
}
       