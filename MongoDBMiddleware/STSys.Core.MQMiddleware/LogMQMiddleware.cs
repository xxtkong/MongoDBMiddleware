using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace STSys.Core.MQMiddleware
{
    public class LogMQMiddleware
    {
        private readonly RequestDelegate _next;
        public LogMQMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            LogMQHelper.Instance.Start();
            await _next(context);
        }
    }
}
