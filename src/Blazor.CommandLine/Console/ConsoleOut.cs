using System.CommandLine;
using System.IO;


namespace Blazor.Components.CommandLine.Console;

public class ConsoleOut : InvocationConfiguration
{
    public ConsoleOut()
    {
        base.Error =  new StringWriter();
        base.Output = new StringWriter();
    }

    public void Write(string value, bool isError = false)
    {
        if (isError)
        {
            base.Error.WriteLine(value);
        }
        else
        {
            base.Output.WriteLine(value);
        }
    }
}