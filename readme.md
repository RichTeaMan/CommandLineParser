# RichTea.CommandLineParser
This project provides a command line parsing utility.

## Cake Tasks
This project uses [Cake](https://cakebuild.net)!
* cake -target=Clean
* cake -target=Restore-Nuget-Packages
* cake -target=Build
* cake -target=Test

## CI

|        | Windows | Linux |
| ------ | --------|-------|
| Master | [![Build status](https://ci.appveyor.com/api/projects/status/gg7er3fta9sjxy5m/branch/master?svg=true)](https://ci.appveyor.com/project/RichTeaMan/commandlineparser/branch/master) | [![Build Status](https://travis-ci.org/RichTeaMan/CommandLineParser.svg?branch=master)](https://travis-ci.org/RichTeaMan/CommandLineParser) |

## Example
```

static void Main(string[] args)
{
    MethodInvoker command = null;
    try
    {
        command = new CommandLineParserInvoker().GetCommand(typeof(Program), args);
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
            command.Invoke();
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
        }
    }
}

[DefaultClCommand]
private void ProgramState([ClArgs("resultDirectory", "rd")]string resultDirectory = "Results")
{
    // program code here...
}

```
