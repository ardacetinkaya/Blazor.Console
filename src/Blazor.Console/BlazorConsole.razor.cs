namespace Blazor.Console
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using System;
    using System.Threading.Tasks;
    public class BlazorConsoleComponent : ComponentBase
    {
        protected string terminal = string.Empty;
        protected string input = string.Empty;

        protected TerminalCommand command = new TerminalCommand();
        protected class TerminalCommand
        {
            public string Text { get; set; }
        }
        protected async Task Execute(EditContext context)
        {
            var cmd = context.Model as TerminalCommand;
            terminal += $"<p><span style='color:white;font-weight:bold'>{DateTime.Now.ToShortTimeString()} > </span><span style='color:yellow'>{cmd.Text}{Environment.NewLine}</span>";
            terminal += $"{Output(cmd.Text)}";
            terminal += $"</p>";
            command.Text = string.Empty;

        }

        string Output(string text)
        {
            string output = string.Empty;
            switch (text)
            {
                case "help":
                    output += $"<p>";
                    output += $"<span style='color:white'>{string.Format("{0,10}", "help")}</span>";
                    output += $"<span style='color:white'>{string.Format("{0,10}", "help")}</span>";
                    output += $"<span style='color:white'>{string.Format("{0,10}", "help")}</span>";
                    output += $"</p>";
                    break;
                default:

                    output += $"<span style='color:red'>{text}: command not found.</span>";

                    break;
            }

            return output;
        }

    }
}
