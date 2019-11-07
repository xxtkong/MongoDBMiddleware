using System;
using System.Collections.Generic;
using System.Text;

namespace STSys.Core.MQMiddleware
{
    public class QueueInfo
    {
        public string level { get; set; }
        public string name { get; set; }
        public object msg { get; set; }
    }
}
