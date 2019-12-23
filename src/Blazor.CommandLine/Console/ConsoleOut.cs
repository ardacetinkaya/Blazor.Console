namespace Blazor.Components.CommandLine.Console
{
    using System.CommandLine;

    public class ConsoleOut : IConsole
    {
        public ConsoleOut()
        {
            Out = new DefaultStreamWriter();
            Error = new DefaultStreamWriter();
        }

        public IStandardStreamWriter Error { get; protected set; }

        public IStandardStreamWriter Out { get; protected set; }

        public bool IsOutputRedirected { get; protected set; }

        public bool IsErrorRedirected { get; protected set; }

        public bool IsInputRedirected { get; protected set; }

    }

}