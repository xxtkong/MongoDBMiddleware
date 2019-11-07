using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace STSys.Core.Log
{
    public class STSysLoggerProvider : ILoggerProvider
    {
        private readonly STSysLoggerConfiguration _config;
        public STSysLoggerProvider(STSysLoggerConfiguration config)
        {
            _config = config;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new STSysLogger(categoryName, _config);
        }
        public void Dispose()
        {
            
        }
    }
}
