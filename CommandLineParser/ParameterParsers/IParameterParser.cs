using System;
using System.Collections.Generic;
using System.Text;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public interface IParameterParser
    {
        /// <summary>
        /// Gets types this class is capable of parsing.
        /// </summary>
        IEnumerable<Type> SupportedTypes { get; }

        ParsedResult ParseParameter(string argumentFlag, string[] arguments);
    }
}
