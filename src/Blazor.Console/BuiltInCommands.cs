namespace Blazor.Console
{
    using Blazor.Console.Command;
    using System.Text;
    using System.Threading.Tasks;

    internal class HelpCommand : IHelpCommand
    {
        public string Output { get; set; }

        public async Task<string> Run()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<p>");
            sb.Append($"<span style='color:white;display:block'>{string.Format("help\t\tShow command line help with available commands.")}</span>");
            sb.Append($"<span style='color:white;display:block'>{string.Format("version\t\tDisplays Blazor.Console version.")}</span>");
            sb.Append($"<span style='color:white;display:block'>{string.Format("os\t\tDisplays the current opearting system.")}</span>");
            sb.Append($"</p>");

            Output = sb.ToString();

            return Output;
        }
    }

    internal class InvalidCommand : IInvalidCommand
    {
        public string Output { get; set; }
        private string _commandText = string.Empty;
        public InvalidCommand(string commandText)
        {
            _commandText = commandText;
        }

        public async Task<string> Run()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<span style='color:red'>{_commandText}: command not found.</span>");

            Output = sb.ToString();

            return Output;
        }
    }

    internal class OSCommand : IOSCommand
    {
        public string Output { get; set; }

        public async Task<string> Run()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<p>");
            sb.Append($"<span style='color:white'>{System.Runtime.InteropServices.RuntimeInformation.OSDescription}</span>");
            sb.Append($"</p>");

            Output = sb.ToString();

            return Output;
        }
    }

    internal class VersionCommand : IVersionCommand
    {
        public string Output { get; set; }

        public async Task<string> Run()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<p>");
            sb.Append($"<span style='color:white'>{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}</span>");
            sb.Append($"</p>");

            Output = sb.ToString();

            return Output;
        }
    }

    internal class ClearCommand : ICommand
    {
        string _console;
        public string Output { get; set; }

        public ClearCommand(string console)
        {
            _console = console;
        }
        public async Task<string> Run()
        {
            _console = Output = string.Empty;
            return Output;
        }
    }


    internal class LongCommand : ILongRunningCommand
    {
        readonly ICommandRunning _loadingService;

        public string Output { get; set; }


        public LongCommand(ICommandRunning loadingService)
        {
            _loadingService = loadingService;
        }

        public async Task<string> Run()
        {
            await _loadingService.StartCommandAsync(async (task) =>
             {
                 task.Maintext = "Execution is started...";
                 var i = 0;

                 while (i < 20)
                 {
                     await Task.Delay(550);
                     task.Subtext = "Progress: " + i++;
                 }

                 task.Maintext = "Execution is completed.";
             }, "console");

            StringBuilder sb = new StringBuilder();
            sb.Append($"<p>");
            sb.Append($"<span style='color:white'>This was a long running command</span>");
            sb.Append($"</p>");
            Output += sb.ToString();

            return Output;
        }

    }
}
