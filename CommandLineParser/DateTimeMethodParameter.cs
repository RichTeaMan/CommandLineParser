using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public class DateTimeMethodParameter : MethodParameter<DateTime>
    {
        public DateTimeMethodParameter(ParameterInfo parameterInfo)
            : base(parameterInfo) { }

        public override DateTime GetTypedParameter(string argument)
        {
            return DateTime.Parse(argument);
        }
    }
}
