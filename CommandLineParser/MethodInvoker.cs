using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RichTea.CommandLineParser
{
    public class MethodInvoker
    {
        public Method Method { get; private set; }
        public object[] Parameters { get; private set; }

        public MethodInvoker(Method method, object[] parameters)
        {
            Method = method;
            Parameters = parameters;
        }

        /// <summary>
        /// Invokes the determined method. If the method is async this will wait for the task to complete before returning.
        /// </summary>
        public void Invoke()
        {
            var result = Method.MethodInfo.Invoke(null, Parameters);

            // async methods have this attribute.
            var asyncAttribute = Method.MethodInfo.CustomAttributes.FirstOrDefault(ca => ca.AttributeType == typeof(AsyncStateMachineAttribute));

            if (asyncAttribute != null && result != null)
            {
                var task = result as Task;
                task.Wait();
            }

        }
    }
}
