using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public class StringMethodParameter : MethodParameter<string>
    {
        public StringMethodParameter(ParameterInfo parameterInfo)
            : base(parameterInfo) { }

        public override string GetTypedParameter(string argument)
        {
            return argument;
        }
    }
}
