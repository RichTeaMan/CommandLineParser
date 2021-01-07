using System;
using System.Linq;

namespace RichTea.CommandLineParser
{
    public class HelpMethod
    {
        public static void PrintHelp(ParseContext parseContext)
        {
            string commandName = parseContext.Args.ElementAtOrDefault(1);

            var methods = parseContext.CommandLineMethods.Where(c => c.Name == commandName).ToArray();

            if (!string.IsNullOrEmpty(commandName))
            {
                if (methods.Any())
                {
                    Console.WriteLine($"{commandName}:");
                    foreach (var method in methods)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"{method.Description}");
                        foreach (var parameter in method.ParameterInfos)
                        {
                            Console.WriteLine($"  - {parameter.Name}");
                        }
                    }
                    return;
                }
                else
                {
                    Console.WriteLine($"No method with name {commandName}.");
                }
            }

            Console.WriteLine("Available commands:");

            foreach (var method in parseContext.CommandLineMethods)
            {
                Console.WriteLine($"\t{method.Name}\t{method.Description}");
            }

        }
    }
}
