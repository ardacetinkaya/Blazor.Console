using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        IServiceProvider _provider;

        public CommandInput(IServiceProvider provider, ICommandRunning runningCommand)
        {
            _provider = provider;
            _runningCommand = runningCommand;
        }

        public async Task<string> Result()
        {
            string output = string.Empty;
            ICommand command=null;
            try
            {
                command = (Text switch
                {
                    "lng" => _ = _provider.GetService<ILongRunningCommand>(),
                    "os" => _ = _provider.GetService<IOSCommand>(),
                    "version" => _ = _provider.GetService<IVersionCommand>(),
                    "help" => _ = _provider.GetService<IHelpCommand>(),
                    _ => _ = new InvalidCommand(Text)
                });
            }
            catch (System.Exception)
            {
                command = new InvalidCommand(Text);
            }
            finally
            {
                output = await command.Run();
            }


            return output;
        }

        public override string ToString()
        {
            return $"<span style='color:white;font-weight:bold'>{Time.ToShortTimeString()} > </span><span style='color:yellow'>{Text}{Environment.NewLine}</span>";
        }
    }
}
