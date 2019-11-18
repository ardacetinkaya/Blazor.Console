using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Console.Command
{
    public interface IRunningCommand
    {
        Task StartCommandAsync(Func<ICommandStatus, Task> action, string maintext = null, string subtext = null);
        void SubscribeToCommandProgressChanged(Action<ICommandStatus> action);
        void UnsubscribeFromCommandProgressChanged(Action<ICommandStatus> action);
    }
}
