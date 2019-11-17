namespace Blazor.Console.Command
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICommand
    {
        string Output { get; set; }
        Task<string> Run(params string[] arguments);
    }

    public interface IHelpCommand:ICommand
    {

    }

    public interface IOSCommand : ICommand
    { }

    public interface IInvalidCommand:ICommand
    {

    }
    public interface IVersionCommand : ICommand
    {

    }

    public interface ILongRunningCommand : ICommand
    {

    }

}
