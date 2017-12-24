using System;
using System.Collections.Generic;
using System.Text;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public abstract class ParameterParser : IParameterParser
    {
        /// <summary>
        /// Gets types this class is capable of parsing.
        /// </summary>
        public abstract IEnumerable<Type> SupportedTypes { get; }

        public abstract ParsedResult ParseParameter(string argumentFlag, string[] arguments);
    }
}
