using System.CommandLine.IO;
using System.Text;

namespace Blazor.Components.CommandLine.Console;

public class DefaultStreamWriter : IStandardStreamWriter
{
    private StringBuilder output = new StringBuilder();
    public DefaultStreamWriter()
    {

    }
    public void Write(string value)
    {
        output.Append(value);
    }

    public override string ToString()
    {
        return output.ToString();
    }
}