namespace Blazor.Components.CommandLine
{
    using System.Threading.Tasks;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Threading;
    using System.Collections.Generic;

    public abstract class BaseCommand
    {
        readonly Command _command;
        readonly bool _longRunning;
        readonly IRunningCommand _loadingService;


        internal Command Command => _command;

        public BaseCommand(string name, string description, bool longRunning = false)
        {
            _longRunning = longRunning;
            _command = new Command(name, description);

            if (_longRunning)
            {
                _loadingService = new RunningCommand();
            }

            Handle();
        }

        private void Handle()
        {
            _command.Handler = CommandHandler.Create<CancellationToken, List<string>, IConsole,int>(this.Run);
        }

        private async Task Run(CancellationToken token, List<string> arguments, IConsole console,int option)
        {
            if (_longRunning)
            {
                await _loadingService.StartCommandAsync(async (task) =>
                {
                    task.Maintext = "Execution is started...";
                    var result = await ExecuteAsync(arguments);
                    task.Maintext = "Execution is completed.";
                    console.Out.WriteLine(result);
                });
            }
            else
            {
                var output = Execute(arguments);
                console.Out.WriteLine(output);
            }

        }
        public virtual async Task<string> ExecuteAsync(List<string> arguments)
        {
            await Task.Delay(200);
            return "ExecuteAsync() is not implemented.";
        }
        public virtual string Execute(List<string> arguments)
        {
            return string.Empty;
        }
        public void AddOption(string[] aliases, string description, string optionArgument)
        {
            if (aliases == null || aliases.Length <= 0) throw new System.ArgumentNullException(nameof(aliases));

            _command.AddOption(new Option(aliases)
            {
                Description = description,
                Argument = new Argument<int>(optionArgument)
            });

            Handle();
        }

        public void AddArgument(string name, string description)
        {
            if (string.IsNullOrEmpty(name)) throw new System.ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(description)) throw new System.ArgumentNullException(nameof(description));

            _command.AddArgument(new Argument<List<string>>
            {
                Name = name,
                Description = description,
                Arity = ArgumentArity.ZeroOrMore
            });

            Handle();
        }


    }
}