using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public class Method
    {
        public MethodParameter[] Parameters { get; protected set; }

        public MethodInfo MethodInfo { get; private set; }
        public string Name { get; private set; }

        static bool IsValidMethod(MethodInfo methodInfo)
        {
            var hasAttr = methodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(ClCommandAttribute));
            if (hasAttr)
            {
                var paras = methodInfo.GetParameters();
                if (paras.Count() == 0)
                    throw new ArgumentException(
                    string.Format("The method '{0}' is a ClCommand, but does not have any paramters.",
                    methodInfo.Name));

                foreach (var para in paras)
                {
                    if (!IsValidParameter(para))
                        throw new ArgumentException(
                            string.Format("The method '{0}' is a ClCommand, but does not have all valid ClArg parameters.",
                            methodInfo.Name));
                }
                return true;
            }
            return false;
        }

        static bool IsValidParameter(ParameterInfo parameterInfo)
        {
            var hasAttr = parameterInfo.CustomAttributes.Any(a => a.AttributeType == typeof(ClArgsAttribute));
            var allowedType = parameterInfo.ParameterType == typeof(int) ||
                parameterInfo.ParameterType == typeof(double) ||
                parameterInfo.ParameterType == typeof(string) ||
                parameterInfo.ParameterType == typeof(DateTime);

            return hasAttr && allowedType;
        }

        public Method(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
            var attr = MethodInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(ClCommandAttribute)) as ClCommandAttribute;
            if (attr == null || string.IsNullOrWhiteSpace(attr.Name))
                throw new ArgumentException("Method is not a ClCommand or it has no name set.");

            Name = attr.Name;

            var parameters = new List<MethodParameter>();
            var paras = methodInfo.GetParameters();
            foreach (var para in paras)
            {
                var param = GetParameter(para);
                if (param != null)
                    parameters.Add(param);
                else
                    throw new ArgumentException(
                    string.Format("The parameter '{0}' uses an unsupported data type.",
                    para.Name));
            }
            Parameters = parameters.ToArray();
        }

        MethodParameter GetParameter(ParameterInfo info)
        {
            if (MatchesType(info.ParameterType, typeof(DateTime)))
                return new DateTimeMethodParameter(info);
            if (MatchesType(info.ParameterType, typeof(double)))
                return new DoubleMethodParameter(info);
            if (MatchesType(info.ParameterType, typeof(int)))
                return new IntMethodParameter(info);
            if (MatchesType(info.ParameterType, typeof(string)))
                return new StringMethodParameter(info);
            if (MatchesType(info.ParameterType, typeof(bool)))
                return new BoolMethodParameter(info);
            return null;
        }

        bool MatchesType(Type parameterType, Type type)
        {
            return (parameterType == type ||
                Nullable.GetUnderlyingType(parameterType) == type ||
                (parameterType.IsArray && parameterType.GetElementType() == type));
        }

        public MethodInvoker GetMethodInvoker(ParsedArgs args)
        {
            var argNames = args.Keys;
            if (args.Verb != Name ||
                // Parameters.Count() != args.Count ||
                Parameters.All(p => p.SupportsArgument(argNames)) == false)
            {
                return null;
            }

            var parameterValues = new List<object>();
            foreach (var para in Parameters.OrderBy(p => p.Position))
            {
                object invokeParam;
                var alias = para.GetSupportedAlias(argNames);
                // if null method argument was an optional argument, so pass default value.
                if (alias == null)
                    invokeParam = para.DefaultValue;
                else
                {
                    // check if parameter is an array
                    if (para.IsArray)
                    {
                        invokeParam = para.GetParameterArray(args[alias].ToArray());
                    }
                    else
                    {
                        invokeParam = para.GetParameter(args[alias].FirstOrDefault());
                    }
                }
                parameterValues.Add(invokeParam);
            }
            var invoker = new MethodInvoker(this, parameterValues.ToArray());
            return invoker;
        }

        public static bool TryGetMethod(MethodInfo methodInfo, out Method method)
        {
            method = null;
            try
            {
                method = new Method(methodInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
