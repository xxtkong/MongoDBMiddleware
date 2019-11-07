using System;
using System.Collections.Generic;
using System.Text;

namespace STSys.Core.MQMiddleware
{
    public class NotificationEventArgs
    {
        public bool Ok { get; set; }
        public string Msg { get; set; }
    }
}
