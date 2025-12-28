using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Collections.Generic;
using System.Linq;
using Blazor.Components.CommandLine.Console;
using System.CommandLine.IO;

namespace Blazor.Components.CommandLine;

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
        _command.SetHandler(async (InvocationContext context) =>
        {
            var args = _command.Arguments.FirstOrDefault(a => a.Name == "args") is Argument<List<string>> argsArgument ? context.ParseResult.GetValueForArgument(argsArgument) : [];

            var console = context.Console;

            string o1 = GetOptionValue(context, "-o1");
            string o2 = GetOptionValue(context, "-o2");
            string o3 = GetOptionValue(context, "-o3");
            string o4 = GetOptionValue(context, "-o4");

            if (_loadingService != null)
            {
                await _loadingService.StartCommandAsync(async (task) =>
                {
                    await ExecuteAsync(console.Out as DefaultStreamWriter, o1, o2, o3, o4, args);
                }, "Execution is started...");
            }
            else
            {
                Execute(console.Out as DefaultStreamWriter, o1, o2, o3, o4, args);

            }
        });
    }

    private string GetOptionValue(InvocationContext context, string alias)
    {
        var option = _command.Options.FirstOrDefault(o => o.HasAlias(alias));
        if (option is Option<string> strOption)
        {
            return context.ParseResult.GetValueForOption(strOption);
        }
        return null;
    }


    protected virtual async Task<bool> ExecuteAsync(DefaultStreamWriter console, string option1, string option2, string option3, string option4, List<string> arguments)
    {
        await Task.Delay(200);
        console.Write("ExecuteAsync() is not implemented.");

        return false;
    }

    protected virtual bool Execute(DefaultStreamWriter console, string option1, string option2, string option3, string option4, List<string> arguments)
    {
        console.Write("Execute() is not implemented.");
        return false;
    }

    protected void AddOption(string name, string description)
    {
        if (string.IsNullOrEmpty(name)) throw new System.ArgumentNullException(nameof(name));

        var optionCount = _command.Options.Count();
        if (optionCount < 4)
        {
            optionCount = optionCount == 0 ? 1 : optionCount + 1;
            var optionName = $"-o{optionCount.ToString()}";
            string[] aliases = [name, optionName];

            var option = new Option<string>(name, description)
            {
                Description = description
            };
            _command.AddOption(option);

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

        _command.Add(new Argument<List<string>>("args")
        {
            Description = description,
            Arity = ArgumentArity.ZeroOrMore
        });

        Handle();
    }


}