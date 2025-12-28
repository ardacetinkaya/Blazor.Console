using Blazor.Components.CommandLine.Console;
using Microsoft.Extensions.Logging;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
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
    public DateTime Time { get; } = DateTime.Now;
    readonly IRunningCommand _runningCommand = runningCommand;
    readonly ILogger<CommandInput> _logger = logger;
    readonly IServiceProvider _provider = provider;

    private Parser _parser;
    private readonly CommandLineBuilder _cmdBuilder = new(new Command(name));

    public CommandInput AddCommand(Command command)
    {
        _cmdBuilder?.Command.AddCommand(command);

        return this;
    }

    public void Init()
    {
        if (_cmdBuilder != null)
        {
            _parser = _cmdBuilder.UseHelp().UseDefaults().Build();
        }
    }

    public async Task<string> Result()
    {
        var console = new ConsoleOut();
        await _parser.InvokeAsync(Text, console);

        return console.Out.ToString();;
    }

    public override string ToString() => $"<span class='header'>{DateTime.Now:HH:mm:ss.fff} > </span><span class='command'>{Text}{Environment.NewLine}</span>";
}
