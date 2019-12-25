namespace Blazor.Components.CommandLine
{
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Threading.Tasks;
    using System.CommandLine.Invocation;
    using System.Threading;

    public class VersionCommand : BaseCommand
    {
        public VersionCommand() : base("version", "Displays Blazor.Commandline version.")
        {

        }
        public override string Execute(string optionArgument1, string optionArgument2, string optionArgument3, string optionArgument4, List<string> arguments)
        {
            try
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }
    }

    public class OSCommand : BaseCommand
    {
        public OSCommand() : base("os", "Displays the current opearting system.")
        {

        }
        public override string Execute(string optionArgument1, string optionArgument2, string optionArgument3, string optionArgument4, List<string> arguments)
        {
            try
            {
                return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }
    }
}
