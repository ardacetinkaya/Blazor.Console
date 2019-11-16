using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Console.Command
{
    public interface ICommandRunning
    {
        Task StartCommandAsync(Func<ICommandStatus, Task> action, string context = "", string maintext = null, string subtext = null);
        void SubscribeToCommandProgressChanged(string context, Action<ICommandStatus> action);
        void UnsubscribeFromCommandProgressChanged(string context, Action<ICommandStatus> action);
    }
}
