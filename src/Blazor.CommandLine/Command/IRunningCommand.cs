namespace Blazor.Components.CommandLine.Command
{
    using System;
    using System.Threading.Tasks;
    public interface IRunningCommand
    {
        Task StartCommandAsync(Func<ICommandStatus, Task> action, string maintext = null, string subtext = null);
        void SubscribeToCommandProgressChanged(Action<ICommandStatus> action);
        void UnsubscribeFromCommandProgressChanged(Action<ICommandStatus> action);
    }
}
