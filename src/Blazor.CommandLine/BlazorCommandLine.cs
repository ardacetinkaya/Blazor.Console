namespace Blazor.Components
{
    using Blazor.Components.CommandLine;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Threading.Tasks;
    public class BlazorCommandLineComponent : ComponentBase, IDisposable
    {
        protected string Output = string.Empty;
        protected string Running = string.Empty;
        protected Input Command = new Input();
        protected string Disabled { get; set; } = null;
        protected string Placeholder { get; set; } = "Enter a command, type 'help' for avaliable commands.";
        [Parameter] public string Name { get; set; }
        [Parameter] public string Description { get; set; }
        [Parameter] public List<BaseCommand> Commands { get; set; }
        [Inject] internal IServiceProvider ServiceProvider { get; set; }
        [Inject] public IRunningCommand RunningCommand { get; set; }
        [Inject] internal ILogger<CommandInput> Logger { get; set; }

        CommandInput cmd;

        public BlazorCommandLineComponent()
        {
            Commands = new List<BaseCommand>();
        }

        protected override void OnInitialized()
        {
            RunningCommand = new RunningCommand();
            RunningCommand.SubscribeToCommandProgressChanged(OnProgressChangedEvent);

            cmd = new CommandInput(Logger, ServiceProvider, RunningCommand, "blzr");


        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                cmd.AddCommand(new OSCommand().Command);
                cmd.AddCommand(new VersionCommand().Command);
                foreach (var baseCommand in Commands)
                {
                    cmd.AddCommand(baseCommand.Command);
                }

                cmd.Init();
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


            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        });

        public string Version() => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        protected async Task Execute(EditContext context)
        {
            var input = context.Model as Input;
            if (!string.IsNullOrEmpty(input.Text))
            {
                cmd.Text = input.Text;
                input.Text = string.Empty;

                if (cmd.Text.ToLower() == "clear" || cmd.Text.ToLower() == "clr")
                {
                    Output = string.Empty;
                }
                else
                {
                    Disabled = "disabled";
                    Placeholder = "Please wait for command to be completed.";
                    Output += $"<p>";
                    Output += $"{cmd.ToString()}";
                    Output += $"{await cmd.Result()}";
                    Output += $"</p>";
                    Disabled = null;
                    Placeholder = "Enter a command, type 'help' for avaliable commands.";
                }
            }
        }

        public void Dispose()
        {
            RunningCommand?.UnsubscribeFromCommandProgressChanged(OnProgressChangedEvent);
        }
    }
}
