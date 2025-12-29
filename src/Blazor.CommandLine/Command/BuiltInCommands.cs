using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.CommandLine.Command;

public class VersionCommand() : BaseCommand("version", "Displays Blazor.Commandline version.")
{
    protected override bool Execute(ConsoleOut console,params KeyValuePair<string, string>[] options)
    {
        try
        {
            console.Write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!.ToString());
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}

public class OsCommand() : BaseCommand("os", "Displays the current operating system.")
{
    protected override Task<bool> ExecuteAsync(ConsoleOut console, CancellationToken token = default, params KeyValuePair<string, string>[] options)
    {
        return base.ExecuteAsync(console, token, options);
    }

    protected override bool Execute(ConsoleOut console, params KeyValuePair<string, string>[] options)
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
