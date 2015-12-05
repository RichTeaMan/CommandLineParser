using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public class ParsedArgs : Dictionary<string, List<string>>
    {
        public string Verb { get; set; }

        public ParsedArgs()
            : base()
        {

        }
    }
}
