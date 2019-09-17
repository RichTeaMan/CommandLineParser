using System;

namespace RichTea.CommandLineParser
{
    /// <summary>
    /// Marks a function that is invokeable via the command line arguments.
    /// </summary>
    /// <remarks>
    /// A function must be given a name (a verb) to be invoked.
    ///
    /// A method with this attribute must be static and public to be invokeable.
    ///
    /// All parameters (if any) must  have an <see cref="ClArgsAttribute"/> attribute and
    /// have a supported type to be matched.
    /// </remarks>
    public class ClCommandAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of this function. This name is the verb that prompts this function.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Constructs a ClCommandAttribute with the given name.
        /// </summary>
        /// <param name="name"></param>
        public ClCommandAttribute(string name) : base()
        {
            Name = name;
        }

        [Obsolete]
        public static MethodInvoker GetCommand(Type type, string[] args)
        {
            var commandLineInvoker = new CommandLineParserInvoker();
            return commandLineInvoker.GetCommand(type, args);
        }

        [Obsolete]
        public static ParsedArgs ParseArgs(string[] args)
        {
            var commandLineInvoker = new CommandLineParserInvoker();
            return commandLineInvoker.ParseArgs(args);
        }
    }
}
