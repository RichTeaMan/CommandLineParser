using System;
using System.Linq;

namespace RichTea.CommandLineParser
{
    public class ClArgsAttribute : Attribute
    {
        public string[] Args { get; protected set; }

        public ClArgsAttribute(params string[] args) : base()
        {
            Args = args.Select(a => a.ToLower()).Distinct().ToArray();
        }
    }
}
