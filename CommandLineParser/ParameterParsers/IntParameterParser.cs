using System;
using System.Collections.Generic;
using System.Linq;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public class IntParameterParser : ParameterParser
    {
        public override IEnumerable<Type> SupportedTypes => new[] { typeof(int) };

        public override ParsedResult ParseParameter(string argumentFlag, string[] arguments)
        {
            var result = new ParsedResult();

            if (arguments.Count() == 1)
            {
                if (int.TryParse(arguments.Single(), out int parsedInt))
                {
                    result.Parameter = parsedInt;
                }
                else
                {
                    result.ErrorOutput.Add(new ParserOutput($"Could not parse as '{arguments.Single()}' as integer."));
                }
            }
            else
            {
                result.ErrorOutput.Add(new ParserOutput($"Invalid number of arguments. Only 1 argument is supported."));
            }
            return result;
        }
    }
}
