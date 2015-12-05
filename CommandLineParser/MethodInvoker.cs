using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public class MethodInvoker
    {
        public Method Method { get; private set; }
        public object[] Parameters { get; private set; }

        public MethodInvoker(Method method, object[] parameters)
        {
            Method = method;
            Parameters = parameters;
        }

        public void Invoke()
        {
            Method.MethodInfo.Invoke(null, Parameters);
        }
    }
}
