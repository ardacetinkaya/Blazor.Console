using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Blazor.Console.Command
{
    public class Input
    {
        public string Text { get; set; }
        public DateTime Time { get; } = DateTime.Now;
    }

    public class CommandInput
    {
        public string Text { get; set; }
        public DateTime Time { get; } = DateTime.Now;

        readonly ICommandRunning _runningCommand;
        readonly IEnumerable<string> _commands;
        readonly ILogger<CommandInput> _logger;
        IServiceProvider _provider;

        public CommandInput(ILogger<CommandInput> logger, IServiceProvider provider, ICommandRunning runningCommand)
        {
            _provider = provider;
            _runningCommand = runningCommand;
            _logger = logger;
            _commands = new List<string>()
            {
                {"os"}
                ,{"help"}
                ,{"version"}
            };
        }

        public async Task<string> Result()
        {
            string output = string.Empty;
            ICommand command = null;
            string[] arguments = null;
            try
            {

                if (Text.Trim().IndexOf(" ") == -1)
                {

                }
                else
                {
                    arguments = Text.Split(' ',2,StringSplitOptions.RemoveEmptyEntries);
                }

                command = (Text switch
                {
                    "lng" => _ = _provider.GetRequiredService<ILongRunningCommand>(),
                    "os" => _ = _provider.GetRequiredService<IOSCommand>(),
                    "version" => _ = _provider.GetRequiredService<IVersionCommand>(),
                    "help" => _ = _provider.GetRequiredService<IHelpCommand>(),
                    _ => _ = new InvalidCommand(Text)
                });
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
