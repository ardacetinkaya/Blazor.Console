namespace Blazor.Components.CommandLine
{
    using System.Threading.Tasks;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Threading;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class BaseCommand
    {
        readonly Command _command;
        readonly IRunningCommand _loadingService;
        internal Command Command => _command;

        public BaseCommand(string name, string description, bool longRunning = false)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new System.ArgumentException($"Command name can not have white space.", nameof(name));

            _command = new Command(name, description);

            if (longRunning)
            {
                _loadingService = new RunningCommand();
            }

            Handle();

        }

        private void Handle()
        {
            _command.Handler = CommandHandler.Create<CancellationToken, List<string>, IConsole, string, string, string, string>(async (token, args, console, o1, o2, o3, o4) =>
                  {
                      if (_loadingService != null)
                      {
                          await _loadingService.StartCommandAsync(async (task) =>
                          {
                              task.Maintext = "Execution is started...";
                              var result = await ExecuteAsync(o1, o2, o3, o4, args);
                              task.Maintext = "Execution is completed.";
                              console.Out.WriteLine(result);
                          });
                      }
                      else
                      {
                          var output = Execute(o1, o2, o3, o4, args);
                          console.Out.WriteLine(output);
                      }
                  });
        }


        public virtual async Task<string> ExecuteAsync(string option1, string option2, string option3, string option4, List<string> arguments)
        {
            await Task.Delay(200);
            return "ExecuteAsync() is not implemented.";
        }
        public virtual string Execute(string option1, string option2, string option3, string option4, List<string> arguments)
        {
           return "Execute() is not implemented.";
        }

        public void AddOption(string name, string description)
        {
            if (string.IsNullOrEmpty(name)) throw new System.ArgumentNullException(nameof(name));

            int optionCount = _command.Options.Count();
            if (optionCount < 4)
            {
                optionCount = optionCount == 0 ? 1 : optionCount + 1;
                var optionName = $"-o{optionCount.ToString()}";
                string[] aliases = new string[] { name, optionName };

                _command.AddOption(new Option(aliases)
                {
                    Description = description,
                    Name = name,
                    Argument = new Argument<string>(optionName)
                });

                Handle();
            }
            else
            {
                return;
            }

        }

        public void UseArguments(string description)
        {
            if (string.IsNullOrEmpty(description)) throw new System.ArgumentNullException(nameof(description));

            _command.AddArgument(new Argument<List<string>>
            {
                Name = "args",
                Description = description,
                Arity = ArgumentArity.ZeroOrMore
            });

            Handle();
        }


    }
}