using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Repository;
using STSys.Core.MQMiddleware;
using System;
using System.Collections.Generic;
using System.Text;

namespace STSys.Core.Log
{
    public class STSysLoggerConfiguration
    {
        public IRepositoryMongoDB<QueueInfo> repositoryMongoDB { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
    }
}
