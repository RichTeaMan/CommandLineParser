using RichTea.CommandLineParser.Exceptions;
using RichTea.CommandLineParser.ParameterParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RichTea.CommandLineParser
{
    public class CommandLineParserInvoker
    {

        public List<IParameterParser> _parameterParserCollection { get; } =
            new List<IParameterParser> {
            new BoolParameterParser(),
            new DateTimeParameterParser(),
            new DoubleParameterParser(),
            new IntParameterParser(),
            new StringParameterParser()
        };

        public IReadOnlyCollection<IParameterParser> ParameterParserCollection => _parameterParserCollection;

        public CommandLineParserInvoker AddParameterParser(IParameterParser parameterParser)
        {
            _parameterParserCollection.Add(parameterParser);
            return this;
        }

        public MethodInvoker GetCommand(Type type, string[] args)
        {
            var pargs = ParseArgs(args);
            var allowedMethods = new List<Method>();
            foreach (var methodInfo in type.GetRuntimeMethods().Where(m => m.IsStatic))
            {
                if (Method.TryGetMethod(methodInfo, ParameterParserCollection, out Method method))
                {
                    allowedMethods.Add(method);
                }
            }

            if (allowedMethods.Count == 0)
            {
                throw new ArgumentException("There are no valid ClCommand methods.");
            }

            if (allowedMethods.Count(m => m.IsDefault) >= 2)
            {
                throw new AmbiguousConfigurationException();
            }

            MethodInvoker resultMethod = null;

            // check for default method
            if (string.IsNullOrEmpty(pargs.Verb) && allowedMethods.Any(m => m.IsDefault))
            {
                resultMethod = allowedMethods.Single(m => m.IsDefault).GetMethodInvoker(pargs);
            }
            else
            {
                foreach (var method in allowedMethods)
                {
                    var invoker = method.GetMethodInvoker(pargs);
                    if (invoker != null)
                    {
                        resultMethod = invoker;
                        break;
                    }
                }
            }

            if (null == resultMethod)
            {
                // no allowed methods found
                throw new ArgumentException("There are no valid methods matching.");
            }
            else
            {
                return resultMethod;
            }
        }

        public ParsedArgs ParseArgs(string[] args)
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
                    if (!parsedArgs.ContainsKey(currentSwitch))
                    {
                        parsedArgs.Add(currentSwitch, switchArgs);
                    }
                }
                else
                {
                    if (!parsedArgs.TryGetValue(currentSwitch, out List<string> switchArgs))
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
