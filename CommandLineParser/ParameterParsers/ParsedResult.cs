using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RichTea.CommandLineParser.ParameterParsers
{
    public class ParsedResult
    {
        public object Parameter { get; set; }

        /// <summary>
        /// Gets if this result has any warnings or errors.
        /// </summary>
        public bool HasWarningOutput => ErrorOutput.Any() || WarningOutput.Any();

        public IList<ParserOutput> ErrorOutput { get; } = new List<ParserOutput>();

        public IList<ParserOutput> WarningOutput { get; } = new List<ParserOutput>();

        public string CreateWarningMessage()
        {
            var output = new StringBuilder();

            ErrorOutput.ToList().ForEach(e => output.AppendLine(e.Message));
            WarningOutput.ToList().ForEach(w => output.AppendLine(w.Message));

            return output.ToString();
        }
    }
}
