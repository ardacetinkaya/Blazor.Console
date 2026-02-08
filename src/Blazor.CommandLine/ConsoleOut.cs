using System;
using System.CommandLine;
using System.IO;


namespace Blazor.CommandLine;

public class ConsoleOut : InvocationConfiguration
{
    /// <summary>
    /// Event that fires each time a line is written to the output.
    /// Subscribers receive the raw text (without trailing newline).
    /// </summary>
    public event Action<string> OnOutput;

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
            if (OnOutput != null)
            {
                // When streaming, push via the callback only — don't also write to
                // the StringWriter, so the caller won't see the same text twice.
                OnOutput.Invoke(value);
            }
            else
            {
                base.Output.WriteLine(value);
            }
        }
    }
}