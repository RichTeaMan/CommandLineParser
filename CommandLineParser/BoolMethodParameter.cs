using System.Reflection;

namespace RichTea.CommandLineParser
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
