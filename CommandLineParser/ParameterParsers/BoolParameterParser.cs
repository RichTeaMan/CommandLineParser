using System;
using System.Collections.Generic;
using System.Reflection;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public class BoolParameterParser : ParameterParser
    {
        public override IEnumerable<Type> SupportedTypes => new[] { typeof(bool) };

        public override ParsedResult ParseParameter(string argumentFlag, string[] arguments)
        {
            var parsedResult = new ParsedResult
            {
                Parameter = true
            };
            return parsedResult;
        }
    }
}
