using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public class DoubleMethodParameter : MethodParameter<double>
    {
        public DoubleMethodParameter(ParameterInfo parameterInfo)
            : base(parameterInfo) { }

        public override double GetTypedParameter(string argument)
        {
            return double.Parse(argument);
        }
    }
}
