using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public class DateTimeParameterParser : ParameterParser
    {
        public override IEnumerable<Type> SupportedTypes => new[] { typeof(DateTime) };

        public override ParsedResult ParseParameter(string argumentFlag, string[] arguments)
        {
            var result = new ParsedResult();

            if (arguments.Count() == 1)
            {
                if (DateTime.TryParse(arguments.Single(), out DateTime parsedDateTime))
                {
                    result.Parameter = parsedDateTime;
                }
                else
                {
                    result.ErrorOutput.Add(new ParserOutput($"Could not parse as '{arguments.Single()}' as datetime."));
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
