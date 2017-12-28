using RichTea.CommandLineParser.ParameterParsers;
using RichTea.CommandLineParser.Tests.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RichTea.CommandLineParser.Tests.ParameterParsers
{
    public class PersonParameterParser : ParameterParser
    {
        public override IEnumerable<Type> SupportedTypes => new[] { typeof(Person) };

        public override ParsedResult ParseParameter(string argumentFlag, string[] arguments)
        {
            var result = new ParsedResult();

            if (arguments.Count() == 2)
            {
                Person person = new Person
                {
                    Forename = arguments[0],
                    Surname = arguments[1]
                };
                result.Parameter = person;
            }
            else
            {
                result.ErrorOutput.Add(new ParserOutput($"Invalid number of arguments. Only exactly 2 arguments are- supported."));
            }
            return result;
        }
    }
}
