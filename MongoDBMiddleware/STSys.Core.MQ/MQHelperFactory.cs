using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STSys.Core.MQ
{
    public class MQHelperFactory
    {
        public static MQHelper Default() =>
            new MQHelper("default");
    }
}
