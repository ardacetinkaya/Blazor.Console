namespace Blazor.Console.Command
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using CommandLine;
    
    public class Input
    {
        public string Text { get; set; }
        public DateTime Time { get; } = DateTime.Now;
    }

    internal class CommandInput
    {
        public string Text { get; set; }
        public DateTime Time { get; } = DateTime.Now;

        public Dictionary<string, ICommand> Commands
        {
            get
            {
                return _commands;
            }
        }

        readonly ICommandRunning _runningCommand;
        readonly Dictionary<string, ICommand> _commands;
        readonly ILogger<CommandInput> _logger;
        readonly IServiceProvider _provider;

        public CommandInput(ILogger<CommandInput> logger, IServiceProvider provider, ICommandRunning runningCommand)
        {
            _provider = provider;
            _runningCommand = runningCommand;
            _logger = logger;

            _commands = new Dictionary<string, ICommand>();

        }

        public Dictionary<string, ICommand> AddCommands(Dictionary<string, ICommand> commands)
        {
            foreach (var command in commands)
            {
                if (!Commands.ContainsKey(command.Key))
                {
                    Commands.Add(command.Key, command.Value);
                }
            }

            return Commands;
        }

        public async Task<string> Result()
        {
            string output = string.Empty;
            ICommand command = null;
            string[] arguments = null;

            try
            {
                command = (Text switch
                {
                    "lng" => _ = _provider.GetRequiredService<ILongRunningCommand>(),
                    "os" => _ = _provider.GetRequiredService<IOSCommand>(),
                    "version" => _ = _provider.GetRequiredService<IVersionCommand>(),
                    "help" => _ = _provider.GetRequiredService<IHelpCommand>(),
                    _ => _ = _commands[Text]
                });

                if(command is IHelpCommand)
                {
                    (command as IHelpCommand).Commands = Commands;
                }
            }
            catch (System.Exception)
            {
                command = new InvalidCommand(Text);
            }
            finally
            {
                output = await command.Run(arguments);
            }


            return output;
        }

        public override string ToString()
        {
            return $"<span class='header'>{Time.ToString("HH:mm")} > </span><span class='command'>{Text}{Environment.NewLine}</span>";
        }
    }
}
