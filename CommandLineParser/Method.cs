using RichTea.CommandLineParser.ParameterParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RichTea.CommandLineParser
{
    public class Method
    {
        public IParameterParser[] ParameterParsers { get; private set; }

        public ParameterInfo[] ParameterInfos { get; private set; }

        public ParsedResult[] ParsedResults { get; private set; }

        public IList<IParameterParser> SelectedParameterParsers { get; } =
            new List<IParameterParser>();

        public MethodInfo MethodInfo { get; private set; }
        public string Name { get; private set; }
        public bool IsDefault { get; private set; }

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

        public Method(MethodInfo methodInfo, IEnumerable<IParameterParser> parameterParsers)
        {
            MethodInfo = methodInfo;
            ParameterParsers = parameterParsers.ToArray();
            ParameterInfos = methodInfo.GetParameters();

            var clCommandAttribute = MethodInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(ClCommandAttribute)) as ClCommandAttribute;
            if (!string.IsNullOrWhiteSpace(clCommandAttribute?.Name))
            {
                Name = clCommandAttribute.Name;
            }

            var defaultClCommand = MethodInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(DefaultClCommand)) as DefaultClCommand;
            if (null != defaultClCommand)
            {
                IsDefault = true;
            }

            if (null == clCommandAttribute && null == defaultClCommand)
            {
                throw new ArgumentException("Method is not a ClCommand.");
            }
        }

        private IParameterParser GetParameterParser(ParameterInfo parameterInfo)
        {
            var pType = parameterInfo.ParameterType;

            Name = parameterInfo.Name;
            var attr = parameterInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(ClArgsAttribute));
            var arg = attr as ClArgsAttribute;
            if (arg == null)
                throw new ArgumentException("The parameter does not have a ClArgs Attribute.");

            IParameterParser parameterParserResult = null;

            foreach(var parameterParser in ParameterParsers)
            {
                if (parameterParser.SupportedTypes.Contains(parameterInfo.ParameterType))
                {
                    parameterParserResult = parameterParser;
                }
                else if (parameterParser.SupportedTypes.Contains(Nullable.GetUnderlyingType(parameterInfo.ParameterType)))
                {
                    parameterParserResult = parameterParser;
                }
                else if (parameterInfo.ParameterType.IsArray &&
                    parameterParser.SupportedTypes.Contains(parameterInfo.ParameterType.GetElementType()))
                {
                    parameterParserResult = parameterParser;
                }
            }

            return parameterParserResult;
        }

        bool MatchesType(Type parameterType, Type type)
        {
            return (parameterType == type ||
                Nullable.GetUnderlyingType(parameterType) == type ||
                (parameterType.IsArray && parameterType.GetElementType() == type));
        }

        public MethodInvoker GetMethodInvoker(ParsedArgs args)
        {
            if (args.Verb != Name)
            {
                return null;
            }

            var parsedResultList = new List<ParsedResult>();
            foreach (var parameterInfo in ParameterInfos)
            {
                var attr = parameterInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(ClArgsAttribute));
                var arg = attr as ClArgsAttribute;

                var parameterParser = GetParameterParser(parameterInfo);
                if (parameterParser != null)
                {
                    bool foundValue = false;
                    foreach(var alias in arg.Args)
                    {
                        if (args.ContainsKey(alias))
                        {
                            ParsedResult parameterValue;
                            if (parameterInfo.ParameterType.IsArray)
                            {
                                List<ParsedResult> parameterValueList = new List<ParsedResult>();
                                foreach(var argElement in args[alias])
                                {
                                    ParsedResult parameterListElement = parameterParser.ParseParameter(alias, new[] { argElement });
                                    parameterValueList.Add(parameterListElement);
                                }

                                var combinedResult = Array.CreateInstance(parameterInfo.ParameterType.GetElementType(), parameterValueList.Count);
                                for(int i = 0; i < parameterValueList.Count; i++)
                                {
                                    combinedResult.SetValue(Convert.ChangeType(
                                        parameterValueList[i].Parameter,
                                        parameterInfo.ParameterType.GetElementType()),
                                        i);
                                }

                                ParsedResult combinedParsedResult = new ParsedResult
                                {
                                    Parameter = combinedResult
                                };
                                foreach(var parsedResult in parameterValueList)
                                {
                                    foreach(var error in parsedResult.ErrorOutput)
                                    {
                                        combinedParsedResult.ErrorOutput.Add(error);
                                    }
                                    foreach (var warning in parsedResult.WarningOutput)
                                    {
                                        combinedParsedResult.WarningOutput.Add(warning);
                                    }
                                }
                                parameterValue = combinedParsedResult;
                            }
                            else
                            {
                                parameterValue = parameterParser.ParseParameter(alias, args[alias].ToArray());
                            }

                            if (parameterValue.ErrorOutput?.Any() == true)
                            {
                                throw new ArgumentException($"Error while parsing parameter '{parameterInfo.Name}': {string.Join(", ", parameterValue.ErrorOutput.Select(e => e.Message))}");
                            }

                            parsedResultList.Add(parameterValue);
                            foundValue = true;
                            break;
                        }
                    }
                    if (!foundValue && parameterInfo.HasDefaultValue)
                    {
                        var defaultParsedResult = new ParsedResult { Parameter = parameterInfo.DefaultValue };
                        parsedResultList.Add(defaultParsedResult);
                    }
                }
                else
                {
                    throw new ArgumentException(
                    string.Format("The parameter '{0}' uses an unsupported data type.",
                    parameterInfo.Name));
                }
            }

            if (parsedResultList.Count != ParameterInfos.Count())
            {
                return null;
            }

            // print warnings and errors
            foreach(var p in parsedResultList.Where(p => p.HasWarningOutput))
            {
                Console.WriteLine(p.CreateWarningMessage());
            }

            object[] methodParameters = parsedResultList.Select(r => r.Parameter).ToArray();

            var invoker = new MethodInvoker(this, methodParameters);
            return invoker;
        }

        public static bool TryGetMethod(MethodInfo methodInfo, IEnumerable<IParameterParser> parameterParsers, out Method method)
        {
            method = null;
            try
            {
                method = new Method(methodInfo, parameterParsers);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
