namespace Blazor.Console
{
    using Blazor.Console.Command;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class BlazorConsoleComponent : ComponentBase, IDisposable
    {
        protected string Output = string.Empty;
        protected string Running = string.Empty;

        protected Input Command = new Input();

        [Parameter] public Dictionary<string, ICommand> Commands { get; set; }
        [Inject] public IServiceProvider ServiceProvider { get; set; }
        [Inject] public ICommandRunning RunningCommand { get; set; }

        public BlazorConsoleComponent()
        {
        }

        protected override void OnInitialized()
        {
            RunningCommand = new CommandRunning();
            RunningCommand.SubscribeToCommandProgressChanged("console", OnProgressChangedEvent);
        }

        private Action<ICommandStatus> OnProgressChangedEvent => new Action<ICommandStatus>((ICommandStatus task) =>
        {
            if (task != null)
            {
                if (!string.IsNullOrEmpty(task.Maintext))
                {
                    Running = $"<p style='margin-top:-55px'><span style='display:block'>{task.Maintext}</span><span style='display:block'>{ task.Subtext}</span></p>";

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
        protected Dictionary<string, object> Attributes()
        {
            var handy = new Dictionary<string, object>();
            handy.Add("autofocus", true);
            return handy;
        }
        protected async Task Execute(EditContext context)
        {
            var input = context.Model as Input;
            if (!string.IsNullOrEmpty(input.Text))
            {
                var cmd = new CommandInput(ServiceProvider, RunningCommand);
                cmd.Text = input.Text;
                input.Text = string.Empty;
                if (cmd.Text.ToLower() == "clear" || cmd.Text.ToLower() == "clr")
                {
                    Output = string.Empty;
                }
                else
                {
                    Output += $"<p>";
                    Output += $"{cmd.ToString()}";
                    Output += $"{await cmd.Result()}";
                    Output += $"</p>";
                }
            }
        }

        public void Dispose()
        {
            RunningCommand?.UnsubscribeFromCommandProgressChanged("console", OnProgressChangedEvent);
        }
    }
}