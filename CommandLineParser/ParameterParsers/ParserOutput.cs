using System;
using System.Collections.Generic;
using System.Text;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public sealed class ParserOutput
    {
        public string Message { get; set; }

        public ParserOutput() { }

        public ParserOutput(string message) : this()
        {
            Message = message;
        }

        public override string ToString()
        {
            return $"Parser output: '{Message}'";
        }
    }
}
