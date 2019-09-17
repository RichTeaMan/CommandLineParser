using System;

namespace RichTea.CommandLineParser
{
    /// <summary>
    /// This attribute is for default command line methods. If no verb is supplised this method will be used.
    ///
    /// </summary>
    /// <remarks>
    /// A run time error will occur if multiple default attributes are used.
    ///
    /// All parameters (if any) must  have an <see cref="ClArgsAttribute"/> attribute and
    /// have a supported type to be matched.
    /// </remarks>
    public class DefaultClCommand : Attribute
    {

    }
}
