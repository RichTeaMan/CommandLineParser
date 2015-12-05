using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public class BoolMethodParameter : MethodParameter<bool>
    {
        public BoolMethodParameter(ParameterInfo parameterInfo)
            : base(parameterInfo)
        {
            Optional = true;
            BaseDefaultValue = false;
        }

        public override bool GetTypedParameter(string argument)
        {
            return true;
        }
    }
}
