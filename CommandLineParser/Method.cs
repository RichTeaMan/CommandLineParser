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

        public MethodInfo MethodInfo { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public bool IsDefault { get; private set; }

        public Method(MethodInfo methodInfo, IEnumerable<IParameterParser> parameterParsers, string name, string description, bool isDefault)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            ParameterParsers = parameterParsers?.ToArray() ?? new IParameterParser[0];
            ParameterInfos = methodInfo.GetParameters();
            Name = name;
            Description = description;
            IsDefault = isDefault;
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

            foreach (var parameterParser in ParameterParsers)
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

        public MethodInvoker GetMethodInvoker(ParseContext parseContext)
        {
            if (parseContext.ParsedArgs.Verb != Name)
            {
                return null;
            }

            var parsedResultList = new List<ParsedResult>();
            foreach (var parameterInfo in ParameterInfos)
            {
                if (parameterInfo.ParameterType == typeof(ParseContext))
                {
                    parsedResultList.Add(new ParsedResult { Parameter = parseContext });
                    continue;
                }
                var attr = parameterInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(ClArgsAttribute));
                var arg = attr as ClArgsAttribute;

                var parameterParser = GetParameterParser(parameterInfo);
                if (parameterParser != null)
                {
                    bool foundValue = false;
                    foreach (var alias in arg.Args)
                    {
                        if (parseContext.ParsedArgs.ContainsKey(alias))
                        {
                            ParsedResult parameterValue;
                            if (parameterInfo.ParameterType.IsArray)
                            {
                                List<ParsedResult> parameterValueList = new List<ParsedResult>();
                                foreach (var argElement in parseContext.ParsedArgs[alias])
                                {
                                    ParsedResult parameterListElement = parameterParser.ParseParameter(alias, new[] { argElement });
                                    parameterValueList.Add(parameterListElement);
                                }

                                var combinedResult = Array.CreateInstance(parameterInfo.ParameterType.GetElementType(), parameterValueList.Count);
                                for (int i = 0; i < parameterValueList.Count; i++)
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
                                foreach (var parsedResult in parameterValueList)
                                {
                                    foreach (var error in parsedResult.ErrorOutput)
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
                                parameterValue = parameterParser.ParseParameter(alias, parseContext.ParsedArgs[alias].ToArray());
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
            foreach (var p in parsedResultList.Where(p => p.HasWarningOutput))
            {
                Console.WriteLine(p.CreateWarningMessage());
            }

            object[] methodParameters = parsedResultList.Select(r => r.Parameter).ToArray();

            var invoker = new MethodInvoker(this, methodParameters);
            return invoker;
        }

        /// <summary>
        /// Gets an invoker with passing or checking parameters.
        /// </summary>
        /// <returns></returns>
        public MethodInvoker GetMethodInvoker()
        {
            var invoker = new MethodInvoker(this, null);
            return invoker;
        }

        public static bool TryGetMethod(MethodInfo methodInfo, IEnumerable<IParameterParser> parameterParsers, out Method method)
        {
            method = null;
            string name = null;
            string description = string.Empty;
            var clCommandAttribute = methodInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(ClCommandAttribute)) as ClCommandAttribute;
            if (!string.IsNullOrWhiteSpace(clCommandAttribute?.Name))
            {
                name = clCommandAttribute.Name;
                description = clCommandAttribute.Description;
            }

            bool isDefault = false;
            var defaultClCommand = methodInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(DefaultClCommand)) as DefaultClCommand;
            if (null != defaultClCommand)
            {
                isDefault = true;
            }

            if (null == clCommandAttribute && null == defaultClCommand)
            {

                return false;
            }

            method = new Method(methodInfo, parameterParsers, name, description, isDefault);
            return true;
        }
    }
}
