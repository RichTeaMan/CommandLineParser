using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLineParser
{
    public class ClCommandAttribute : Attribute
    {
        public string Name { get; set; }

        public ClCommandAttribute(string name) : base()
        {
            Name = name;
        }

        public static MethodInvoker GetCommand(Type type, string[] args)
        {
            var pargs = ParseArgs(args);
            var allowedMethods = new List<Method>();
            foreach (var methodInfo in type.GetMethods().Where(m => m.IsStatic))
            {
                Method method;
                if (Method.TryGetMethod(methodInfo, out method))
                {
                    allowedMethods.Add(method);
                }
            }

            if (allowedMethods.Count == 0)
                throw new ArgumentException("There are no valid ClCommand methods.");

            foreach (var method in allowedMethods)
            {
                var invoker = method.GetMethodInvoker(pargs);
                if (invoker != null)
                    return invoker;
            }
            // no allowed methods found
            throw new ArgumentException("There are no valid methods matching.");
        }

        static bool IsValidMethod(MethodInfo methodInfo)
        {
            var hasAttr = methodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(ClCommandAttribute));
            if (hasAttr)
            {
                var paras = methodInfo.GetParameters();
                if(paras.Count() == 0)
                    throw new ArgumentException(
                    string.Format("The method '{0}' is a ClCommand, but does not have any paramters.",
                    methodInfo.Name));

                foreach (var para in paras)
                {
                    if(!IsValidParameter(para))
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
                parameterInfo.ParameterType == typeof(DateTime) ||
                parameterInfo.ParameterType == typeof(int?) ||
                parameterInfo.ParameterType == typeof(double?) ||
                parameterInfo.ParameterType == typeof(DateTime?);

            return hasAttr && allowedType;
        }

        static ParsedArgs ParseArgs(string[] args)
        {
            var parsedArgs = new ParsedArgs();

            // The first argument is always the 'verb' unless it starts with a hyphen.
            var first = args.FirstOrDefault();
            int skip = 0;
            if (first != null && !first.StartsWith("-"))
            {
                parsedArgs.Verb = first;
                skip = 1;
            }
            
            string currentSwitch = "";
            foreach (var arg in args.Skip(skip))
            {
                if (arg.StartsWith("-"))
                {
                    // arguments to a switch can be space or colon seperated. Colon args are found first.
                    var splits = arg.Split(':');
                    currentSwitch = splits.First().Substring(1).ToLower();

                    var switchArgs = splits.Skip(1).ToList();
                    parsedArgs.Add(currentSwitch, switchArgs);
                }
                else
                {
                    List<string> switchArgs;
                    if (!parsedArgs.TryGetValue(currentSwitch, out switchArgs))
                    {
                        switchArgs = new List<string>();
                        parsedArgs.Add(currentSwitch, switchArgs);
                    }
                    switchArgs.Add(arg);
                }
            }
            return parsedArgs;
        }
    }
}
