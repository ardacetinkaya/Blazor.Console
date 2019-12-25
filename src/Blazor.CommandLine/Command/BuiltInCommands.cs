namespace Blazor.Components.CommandLine
{
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Threading.Tasks;
    using System.CommandLine.Invocation;
    using System.Threading;
    using Blazor.Components.CommandLine.Console;

    public class VersionCommand : BaseCommand
    {
        public VersionCommand() : base("version", "Displays Blazor.Commandline version.")
        {

        }
        public override bool Execute(DefaultStreamWriter console,string optionArgument1, string optionArgument2, string optionArgument3, string optionArgument4, List<string> arguments)
        {
            try
            {
                console.Write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }

    public class OSCommand : BaseCommand
    {
        public OSCommand() : base("os", "Displays the current opearting system.")
        {

        }
        public override bool Execute(DefaultStreamWriter console,string optionArgument1, string optionArgument2, string optionArgument3, string optionArgument4, List<string> arguments)
        {
            try
            {
                console.Write(System.Runtime.InteropServices.RuntimeInformation.OSDescription);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
