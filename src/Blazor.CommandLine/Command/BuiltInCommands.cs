using System.Collections.Generic;
using System.Threading.Tasks;
using Blazor.Components.CommandLine.Console;

namespace Blazor.Components.CommandLine;

public class VersionCommand : BaseCommand
{
    public VersionCommand() : base("version", "Displays Blazor.Commandline version.")
    {

    }

    protected override bool Execute(DefaultStreamWriter console,string optionArgument1, string optionArgument2, string optionArgument3, string optionArgument4, List<string> arguments)
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


    protected override Task<bool> ExecuteAsync(DefaultStreamWriter console, string option1, string option2, string option3, string option4, List<string> arguments)
    {
        return base.ExecuteAsync(console, option1, option2, option3, option4, arguments);
    }

    protected override bool Execute(DefaultStreamWriter console, string optionArgument1, string optionArgument2, string optionArgument3, string optionArgument4, List<string> arguments)
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
