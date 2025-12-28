using System.CommandLine.IO;
using System.Text;

namespace Blazor.Components.CommandLine.Console;

public class DefaultStreamWriter : IStandardStreamWriter
{
    private readonly StringBuilder _output = new StringBuilder();
    public DefaultStreamWriter()
    {

    }
    public void Write(string value)
    {
        _output.Append(value);
    }

    public override string ToString()
    {
        return _output.ToString();
    }
}