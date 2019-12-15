namespace Blazor.Components.CommandLine.Command
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommand
    {
        string Output { get; set; }
        string Help { get;}
        Task<string> Run(params string[] arguments);
    }

    interface IHelpCommand:ICommand
    {
        Dictionary<string, ICommand> Commands { get; set; }
    }

    interface IOSCommand : ICommand
    { }

    interface IInvalidCommand:ICommand
    {

    }
    interface IVersionCommand : ICommand
    {

    }

    interface ILongRunningCommand : ICommand
    {

    }

}
