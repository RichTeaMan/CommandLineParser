using System;
using System.Collections.Generic;
using System.Linq;

namespace RichTea.CommandLineParser
{
    public class ParseContext
    {
        public IReadOnlyList<string> Args { get; }
        
        public ParsedArgs ParsedArgs { get; }

        public IReadOnlyList<Method> CommandLineMethods { get; }


        public ParseContext(IEnumerable<string> args, ParsedArgs parsedArgs, IEnumerable<Method> commandLineMethods)
        {
            Args = args?.ToArray() ?? throw new ArgumentNullException(nameof(args));
            ParsedArgs = parsedArgs ?? throw new ArgumentNullException(nameof(parsedArgs));
            CommandLineMethods = commandLineMethods?.ToArray() ?? throw new ArgumentNullException(nameof(commandLineMethods));
        }


    }
}
