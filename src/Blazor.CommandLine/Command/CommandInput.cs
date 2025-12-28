using Blazor.Components.CommandLine.Console;
using Microsoft.Extensions.Logging;
using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Blazor.Components.CommandLine;

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

    public CommandInput AddCommand(Command command)
    {
        _cmdBuilder?.Subcommands.Add(command);

        return this;
    }
    
    public async Task<string> Result()
    {
        var console = new ConsoleOut();
        await _cmdBuilder.Parse(Text).InvokeAsync(console);
        return console.Output.ToString();;
    }

    public override string ToString() => $"<span class='header'>{DateTime.Now:HH:mm:ss.fff} > </span><span class='command'>{Text}{Environment.NewLine}</span>";
}
