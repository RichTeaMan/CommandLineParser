using System;
using System.Collections.Generic;
using System.Text;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public class ParsedResult
    {
        public object Parameter { get; set; }

        public IList<ParserOutput> ErrorOutput { get; } = new List<ParserOutput>();

        public IList<ParserOutput> WarningOutput { get; } = new List<ParserOutput>();
    }
}
