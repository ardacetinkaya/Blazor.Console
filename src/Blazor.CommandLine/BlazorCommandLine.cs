using Blazor.Components.CommandLine;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.Components;

public class BlazorCommandLineComponent : ComponentBase, IDisposable
{
    protected string Output = string.Empty;
    protected string Running = string.Empty;
    protected readonly Input Command = new Input();
    protected string Disabled { get; set; } = null;
    protected string Placeholder { get; set; } = "Enter a command, type 'help' for available commands.";
    [Parameter] public string Name { get; set; }
    [Parameter] public string Description { get; set; }
    [Parameter] public bool ShowDate { get; set; } = true;
    [Parameter] public List<BaseCommand> Commands { get; set; } = [];
    [Inject] internal IServiceProvider ServiceProvider { get; set; }
    [Inject] public IRunningCommand RunningCommand { get; set; }
    [Inject] internal ILogger<CommandInput> Logger { get; set; }

    private CommandInput _cmd;

    protected override void OnInitialized()
    {
        RunningCommand = new RunningCommand();
        RunningCommand.SubscribeToCommandProgressChanged(OnProgressChangedEvent);

        _cmd = new CommandInput(Logger, ServiceProvider, RunningCommand, "blzr");


    }
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _cmd.AddCommand(new OsCommand().Command);
            _cmd.AddCommand(new VersionCommand().Command);
            foreach (var baseCommand in Commands)
            {
                _cmd.AddCommand(baseCommand.Command);
            }
        }
    }

    private Action<ICommandStatus> OnProgressChangedEvent => new Action<ICommandStatus>((ICommandStatus task) =>
    {
        if (task != null)
        {
            if (!string.IsNullOrEmpty(task.Maintext))
            {
                Running = $"<p class='prgs'><span class='main'>{task.Maintext}</span><span class='subtext'>{task.Subtext}</span></p>";
            }
        }
        else
        {
            Running = string.Empty;
        }


        InvokeAsync(StateHasChanged);
    });

    protected string Version() => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

    protected async Task Execute(EditContext context)
    {
        var input = context.Model as Input;
        if (!string.IsNullOrEmpty(input.Text))
        {
            _cmd.Text = input.Text;
            input.Text = string.Empty;

            if (_cmd.Text.ToLower() == "clear" || _cmd.Text.ToLower() == "clr")
            {
                Output = string.Empty;
            }
            else
            {
                Disabled = "disabled";
                Placeholder = "Please wait for command to be completed.";
                Output += $"<p>";
                Output += $"{_cmd.ToString()}";
                Output += $"{await _cmd.Result()}";
                Output += $"</p>";
                Disabled = null;
                Placeholder = "Enter a command, type '--help' for available commands.";
            }
        }
    }

    public void Dispose() => RunningCommand?.UnsubscribeFromCommandProgressChanged(OnProgressChangedEvent);
}
