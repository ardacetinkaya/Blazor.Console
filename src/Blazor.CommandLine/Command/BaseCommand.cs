using System.Threading.Tasks;
using System.CommandLine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Blazor.CommandLine.Command;

public abstract class BaseCommand
{
    private readonly IRunningCommand _loadingService;
    internal System.CommandLine.Command Command { get; }

    protected BaseCommand(string name, string description, bool longRunning = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new System.ArgumentException($"Command name can not have white space.", nameof(name));

        Command = new System.CommandLine.Command(name, description);

        if (longRunning)
        {
            _loadingService = new RunningCommand();
        }

        Handle();
    }

    private void Handle()
    {
        if (_loadingService != null)
        {
            Command.SetAction((parseResult, cancellationToken) =>
            {
                var options = GetOptionsValue(parseResult);
                var console = parseResult.InvocationConfiguration as ConsoleOut;
                return _loadingService.StartCommandAsync(
                    async (task) => { await ExecuteAsync(console, cancellationToken, options.ToArray()); },
                    "Execution is started...");
            });
        }
        else
        {
            Command.SetAction(parseResult =>
            {
                var options = GetOptionsValue(parseResult);
                var console = parseResult.InvocationConfiguration as ConsoleOut;


                Execute(console, options.ToArray());
            });
        }
    }

    private IEnumerable<KeyValuePair<string, string>> GetOptionsValue(ParseResult context)
    {
        foreach (var option in Command.Options?.AsQueryable()!)
        {
            if (option is not Option<string> strOption) continue;

            var optionValue = context.GetValue<string>(strOption);

            if (!string.IsNullOrEmpty(optionValue))
            {
                yield return new KeyValuePair<string, string>(option.Name.Trim('-'),
                    context.GetValue<string>(strOption));
            }
        }
    }


    protected virtual async Task<bool> ExecuteAsync(ConsoleOut console, CancellationToken cancellationToken = default,
        params KeyValuePair<string, string>[] options)
    {
        await Task.Delay(200, cancellationToken);
        console.Write("ExecuteAsync() is not implemented.");

        return false;
    }

    protected virtual bool Execute(ConsoleOut console, params KeyValuePair<string, string>[] options)
    {
        console.Write("Execute() is not implemented.");
        return false;
    }

    protected void AddOption(string name, string description)
    {
        if (string.IsNullOrEmpty(name)) throw new System.ArgumentNullException(nameof(name));
        
        // Normalize option name as --option
        name = $"--{name.Trim('-')}";
        
        // Check for duplicate option names
        if(Command.Options.Any(o => o.Name==name))
        {
            throw new System.ArgumentException($"Option with name '{name}' already exists in command '{Command.Name}'.");
        }

        var optionCount = Command.Options.Count;
        optionCount = optionCount == 0 ? 1 : optionCount + 1;

        // Create aliases for the option with a short form -o1, -o2, etc.
        string[] aliases = [$"-o{optionCount.ToString()}"];

        var option = new Option<string>(name, aliases)
        {
            Description = description,
        };
        
        Command.Options.Add(option);

        
        Handle();
    }

    public void UseArguments(string description)
    {
        if (string.IsNullOrEmpty(description)) throw new System.ArgumentNullException(nameof(description));

        Command.Add(new Argument<List<string>>("args")
        {
            Description = description,
            Arity = ArgumentArity.ZeroOrMore
        });

        Handle();
    }
}