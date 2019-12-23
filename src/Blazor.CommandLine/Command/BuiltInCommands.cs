namespace Blazor.Components.CommandLine
{
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Threading.Tasks;
    using System.CommandLine.Invocation;
    using System.Threading;

    public class VersionCommand : Command
    {
        public VersionCommand() : base("version", "Displays Blazor.Commandline version.")
        {
            this.Handler = CommandHandler.Create<CancellationToken, IConsole>(this.Run);
        }
        public async Task<int> Run(CancellationToken ct, IConsole console)
        {
            try
            {
                console.Out.Write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                return await Task.FromResult(0);
            }
            catch (System.Exception)
            {
                return 1;
            }
        }
    }

    public class OSCommand : Command
    {
        public OSCommand() : base("os", "Displays the current opearting system.")
        {
            this.Handler = CommandHandler.Create<CancellationToken, IConsole>(this.Run);
        }
        public async Task<int> Run(CancellationToken ct, IConsole console)
        {
            try
            {
                console.Out.Write(System.Runtime.InteropServices.RuntimeInformation.OSDescription);
                return await Task.FromResult(0);
            }
            catch (System.Exception)
            {
                return 1;
            }
        }
    }

    public class PrintCommand : Command
    {
        public PrintCommand(string name, string description = null) : base(name, description)
        {
            this.AddOption(new Option(new[] { "-d", "--destination" })
            {
                Description = "Get print destination. 0:Excel 1:CSV 2:PDF",
                Argument = new Argument<int>("pid"),
            });

            this.AddArgument(new Argument<List<string>>
            {
                Name = "columns",
                Description = $"Add column names for output report.Columns: Name Origin Code Price StockDate Count",
                Arity = ArgumentArity.ZeroOrMore
            });

            this.Handler = CommandHandler.Create<CancellationToken, List<string>, IConsole, int>(this.Run);
        }

        public async Task<int> Run(CancellationToken ct, List<string> columns, IConsole console, int productId)
        {
            try
            {
                console.Out.Write("HELLO");
                return await Task.FromResult(0);
            }
            catch (System.Exception)
            {
                return 1;
            }
        }

    }


}
