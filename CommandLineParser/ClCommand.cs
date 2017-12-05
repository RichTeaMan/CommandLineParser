using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RichTea.CommandLineParser
{
    public class ClCommandAttribute : Attribute
    {
        public string Name { get; private set; }

        public ClCommandAttribute(string name) : base()
        {
            Name = name;
        }

        [Obsolete]
        public static MethodInvoker GetCommand(Type type, string[] args)
        {
            var commandLineInvoker = new CommandLineParserInvoker();
            return commandLineInvoker.GetCommand(type, args);
        }

        [Obsolete]
        public static ParsedArgs ParseArgs(string[] args)
        {
            var commandLineInvoker = new CommandLineParserInvoker();
            return commandLineInvoker.ParseArgs(args);
        }
    }
}
