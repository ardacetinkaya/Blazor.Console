namespace Blazor.Components.CommandLine
{
    using Blazor.Components.CommandLine.Console;
    using Microsoft.Extensions.Logging;
    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Parsing;
    using System.Threading.Tasks;

    public class Input
    {
        public string Text { get; set; }
        public DateTime Time { get; } = DateTime.Now;
    }

    internal class CommandInput
    {
        public string Text { get; set; }
        public DateTime Time { get; } = DateTime.Now;
        readonly IRunningCommand _runningCommand;
        readonly ILogger<CommandInput> _logger;
        readonly IServiceProvider _provider;

        Parser _parser;
        readonly CommandLineBuilder _cmdBuilder;
        public CommandInput(ILogger<CommandInput> logger, IServiceProvider provider, IRunningCommand runningCommand,string name)
        {
            _provider = provider;
            _runningCommand = runningCommand;
            _logger = logger;
            
            _cmdBuilder = new CommandLineBuilder(new Command(name));
        }

        public CommandInput AddCommand(Command command)
        {
            if (_cmdBuilder != null)
            {
                _cmdBuilder.AddCommand(command);
            }

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

        public override string ToString()
        {
            return $"<span class='header'>{Time.ToString("HH:mm")} > </span><span class='command'>{Text}{Environment.NewLine}</span>";
        }
    }
}
