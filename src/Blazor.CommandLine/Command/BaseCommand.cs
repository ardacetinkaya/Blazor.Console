using System.Threading.Tasks;
using System.CommandLine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Blazor.Components.CommandLine.Console;

namespace Blazor.Components.CommandLine;

public abstract class BaseCommand
{
    private readonly IRunningCommand _loadingService;
    internal Command Command { get; }

    protected BaseCommand(string name, string description, bool longRunning = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new System.ArgumentException($"Command name can not have white space.", nameof(name));

        Command = new Command(name, description);

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
        var options = Command.Options.AsQueryable();

        foreach (var option in options)
        {
            if (option is not Option<string> strOption) continue;

            var optionValue = context.GetValue<string>(strOption);

            if (!string.IsNullOrEmpty(optionValue))
            {
                yield return new KeyValuePair<string, string>(option.Name.Trim('-'), context.GetValue<string>(strOption));
            }
        }
    }


    protected virtual async Task<bool> ExecuteAsync(ConsoleOut console, CancellationToken cancellationToken = default,
        params KeyValuePair<string, string>[] options)
    {
        await Task.Delay(200);
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

        
        name = $"--{name.Trim('-')}";

        var optionCount = Command.Options.Count();
        if (optionCount < 4)
        {
            optionCount = optionCount == 0 ? 1 : optionCount + 1;
            var optionName = $"-o{optionCount.ToString()}";
            string[] aliases = [name, optionName];

            var option = new Option<string>(name, aliases)
            {
                Description = description,
            };
            Command.Options.Add(option);

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

        Command.Add(new Argument<List<string>>("args")
        {
            Description = description,
            Arity = ArgumentArity.ZeroOrMore
        });

        Handle();
    }
}