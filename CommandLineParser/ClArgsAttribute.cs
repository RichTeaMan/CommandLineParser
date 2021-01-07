using System;
using System.Linq;

namespace RichTea.CommandLineParser
{
    public class ClArgsAttribute : Attribute
    {
        public string[] Args { get; protected set; }

        public string Description { get; private set; } = string.Empty;

        public ClArgsAttribute(params string[] args) : base()
        {
            Args = args.Select(a => a.ToLower()).Distinct().ToArray();
        }
    }
}
