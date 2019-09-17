# RichTea.CommandLineParser
Documentation for [RichTea.CommandLineParser](https://github.com/RichTeaMan/CommandLineParser).

## Basic Usage

```C#

static void Main(string[] args)
{
    Parser.ParseCommand<Program>(args);
}

// methods must be public static
[DefaultClCommand]
public static void ProgramState([ClArgs("resultDirectory", "rd")]string resultDirectory = "Results")
{
    // program code here...
}

```

[Parser.ParseCommand](~/api/RichTea.CommandLineParser.Parser.html) and invokes a command from the given args with simple error handling.

The generic argument is an object that should have functions with command
attribute ([ClCommandAttribute](~/api/RichTea.CommandLineParser.ClArgsAttribute.html),
[DefaultClCommand](~/api/RichTea.CommandLineParser.DefaultClCommand.html)).
Functions should also be static and public.
The annotation, along with ClArgs attributes determine how the function interacts with
the function gets invoked. If a matching function can't be found a message
is printed to the console and -1 is returned.

If the invoke function is successful and returns an integer ParseInteger will return
that value. Otherwise it will return 0.
