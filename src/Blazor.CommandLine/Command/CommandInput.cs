using Microsoft.Extensions.Logging;
using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Blazor.CommandLine.Command;

public class Input
{
    public string Text { get; set; }
    public DateTime Time { get; } = DateTime.Now;
}

internal class CommandInput(
    ILogger<CommandInput> logger,
    IServiceProvider provider,
    IRunningCommand runningCommand,
    string name)
{
    public string Text { get; set; }

    
    private readonly RootCommand _cmdBuilder = new(name);

    public CommandInput AddCommand(System.CommandLine.Command command)
    {
        _cmdBuilder?.Subcommands.Add(command);

        return this;
    }
    
    public async Task<string> Result(Action<string> onStreamOutput = null)
    {
        var console = new ConsoleOut();
        if (onStreamOutput != null)
        {
            console.OnOutput += onStreamOutput;
        }

        try
        {
            var input = Text?.Trim() ?? string.Empty;

            // Handle "help" as an alias for "--help"
            if (string.Equals(input, "help", StringComparison.OrdinalIgnoreCase))
            {
                input = "--help";
            }
            else if (input.StartsWith("help ", StringComparison.OrdinalIgnoreCase))
            {
                // "help <command>" → "<command> --help"
                input = input.Substring(5).Trim() + " --help";
            }

            var parseResult = _cmdBuilder.Parse(input);

            // If there are parse errors (unrecognized command, etc.), show help instead
            if (parseResult.Errors.Count > 0)
            {
                parseResult = _cmdBuilder.Parse("--help");
            }

            await parseResult.InvokeAsync(console);
        }
        finally
        {
            if (onStreamOutput != null)
            {
                console.OnOutput -= onStreamOutput;
            }
        }

        // Always return any output written directly to the StringWriter by System.CommandLine
        // (e.g. help text, parse error messages). The streaming callback only captures
        // our explicit Write() calls, not System.CommandLine's built-in output.
        return console.Output.ToString();
    }

    public override string ToString() => $"<span class='header'>{DateTime.Now:HH:mm:ss.fff} > </span><span class='command'>{Text}{Environment.NewLine}</span>";
}
