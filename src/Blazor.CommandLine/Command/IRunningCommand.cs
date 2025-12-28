using System;
using System.Threading.Tasks;

namespace Blazor.Components.CommandLine;

public interface IRunningCommand
{
    Task StartCommandAsync(Func<ICommandStatus, Task> action, string maintext = null, string subtext = null);
    void SubscribeToCommandProgressChanged(Action<ICommandStatus> action);
    void UnsubscribeFromCommandProgressChanged(Action<ICommandStatus> action);
}
