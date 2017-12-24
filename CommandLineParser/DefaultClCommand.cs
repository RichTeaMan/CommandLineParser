using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RichTea.CommandLineParser
{

    /// <summary>
    /// This attribute is for default command line methods. If no verb is supplised this method will be used.
    /// 
    /// A run time error will occur if multiple default attributes are used.
    /// </summary>
    public class DefaultClCommand : Attribute
    {

    }
}
