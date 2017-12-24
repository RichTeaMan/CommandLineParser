using System;
using System.Collections.Generic;
using System.Linq;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public class StringParameterParser : ParameterParser
    {
        public override IEnumerable<Type> SupportedTypes => new[] { typeof(string) };

        public override ParsedResult ParseParameter(string argumentFlag, string[] arguments)
        {
            var result = new ParsedResult
            {
                Parameter = string.Join(" ", arguments)
            };
            return result;
        }
    }
}
