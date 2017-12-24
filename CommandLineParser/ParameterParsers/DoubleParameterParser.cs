using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public class DoubleParameterParser : ParameterParser
    {
        public override IEnumerable<Type> SupportedTypes => new[] { typeof(double) };

        public override ParsedResult ParseParameter(string argumentFlag, string[] arguments)
        {
            var result = new ParsedResult();

            if (arguments.Count() == 1)
            {
                if (double.TryParse(arguments.Single(), out double parsedDouble))
                {
                    result.Parameter = parsedDouble;
                }
                else
                {
                    result.ErrorOutput.Add(new ParserOutput($"Could not parse as '{arguments.Single()}' as double."));
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
