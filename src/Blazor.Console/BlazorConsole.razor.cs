namespace Blazor.Console
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using System;
    using System.Threading.Tasks;
    public class BlazorConsoleComponent : ComponentBase
    {
        protected string terminal = string.Empty;


        protected TerminalCommand command = new TerminalCommand();

        protected async Task Execute(EditContext context)
        {
            var cmd = context.Model as TerminalCommand;
            terminal += $"<p><span style='color:white;font-weight:bold'>{cmd.Time.ToShortTimeString()} > </span><span style='color:yellow'>{cmd.Text}{Environment.NewLine}</span>";
            terminal += $"{Output(cmd.Text)}";
            terminal += $"</p>";
            command.Text = string.Empty;

        }

        string Output(string text)
        {
            string output = string.Empty;
            switch (text)
            {
                case "os":
                    output += $"<p>";
                    output += $"<span style='color:white'>{System.Runtime.InteropServices.RuntimeInformation.OSDescription}</span>";
                    output += $"</p>";
                    break;
                case "version":
                    output += $"<p>";
                    output += $"<span style='color:white'>{System.Reflection.Assembly.GetExecutingAssembly()}</span>";
                    output += $"</p>";
                    break;
                case "help":
                    output += $"<p>";
                    output += $"<span style='color:white;display:block'>{string.Format("help\t\tDisplays commands that are available")}</span>";
                    output += $"<span style='color:white;display:block'>{string.Format("version\t\tDisplays commands that are available")}</span>";
                    output += $"<span style='color:white;display:block'>{string.Format("os\t\tDisplays commands that are available")}</span>";
                    output += $"</p>";
                    break;
                default:

                    output += $"<span style='color:red'>{text}: command not found.</span>";

                    break;
            }

            return output;
        }

    }
    public class TerminalCommand
    {
        public string Text { get; set; }
        public DateTime Time { get; } = DateTime.Now;
    }
}
