using System;

namespace RichTea.CommandLineParser
{
    public class Parser
    {
        /// <summary>
        /// Parses and invokes a command from the given args with simple error handling.
        /// </summary>
        /// <remarks>
        /// <typeparamref name="T"/> is an object that should have functions with command
        /// attributes (<see cref="ClCommandAttribute"/>, <see cref="DefaultClCommand"/>).
        /// Functions should be static and public.
        /// These attributes determine how the function interacts with <paramref name="args"/>
        /// and which function gets invoked. If a matching function can't be found a message
        /// is printed to the console and -1 is returned.
        /// 
        /// If the invoked function throws an exception ParseCommand will print the exception
        /// and all inner exceptions to the console and return 1.
        /// 
        /// If the invoke function is successful and returns an integer ParseInteger will return
        /// that value. Otherwise it will return 0.
        /// </remarks>
        /// <typeparam name="T">Type containing functions with command attributes.</typeparam>
        /// <param name="args">Program arguments. This is expected to be the arguments passed to main.</param>
        /// <returns>
        /// * 0: Function ran successfully.
        /// * -1: No matching function found.
        /// * 1: Function threw an exception.
        /// 
        /// Other values may also be returned if the invoked function returns an integer.
        /// </returns>
        public static int ParseCommand<T>(string[] args)
        {
            MethodInvoker command = null;
            try
            {
                command = new CommandLineParserInvoker().GetCommand(typeof(T), args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing command:");
                Console.WriteLine(ex);
            }
            if (command != null)
            {
                try
                {
                    object result = command.Invoke();
                    if (result is int)
                    {
                        return (int)result;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error running command:");
                    Console.WriteLine(ex);

                    var inner = ex.InnerException;
                    while (inner != null)
                    {
                        Console.WriteLine(inner);
                        Console.WriteLine();
                        inner = inner.InnerException;
                    }

                    Console.WriteLine(ex.StackTrace);

                    return 1;
                }
            }
            return -1;
        }

    }
}
