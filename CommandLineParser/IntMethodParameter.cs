using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public class IntMethodParameter : MethodParameter<int>
    {
        public IntMethodParameter(ParameterInfo parameterInfo)
            : base(parameterInfo) { }

        public override int GetTypedParameter(string argument)
        {
            return int.Parse(argument);
        }
    }
}
