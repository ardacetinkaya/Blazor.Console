using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.CommandLine.Command;

public class RunningCommand : IRunningCommand
{
    private static readonly ConcurrentDictionary<string, TaskContext> Tasks = new ConcurrentDictionary<string, TaskContext>();

    public async Task StartCommandAsync(Func<ICommandStatus, Task> action, string maintext = null, string subtext = null)
    {
        var task = new RunningTask("console", maintext, subtext);
        if (!Tasks.TryGetValue("console", out TaskContext c))
        {
            c = new TaskContext
            {
                Tasks = { task }
            };
            Tasks.TryAdd("console", c);
        }
        else
        {
            lock (c)
            {
                c.Tasks.Add(task);
            }
            c.FireChanged();
        }

        try
        {
            await action(task);
            task.Dispose();
        }
        catch (Exception ex)
        {
            task.Exception = ex;
        }
    }

    public void SubscribeToCommandProgressChanged(Action<ICommandStatus> action)
    {
        if (!Tasks.TryGetValue("console", out TaskContext c))
        {
            c = new TaskContext();
            Tasks.TryAdd("console", c);
        }

        c.Changed += action;
        c.FireChanged();
    }

    public void UnsubscribeFromCommandProgressChanged(Action<ICommandStatus> action)
    {
        if (Tasks.TryGetValue("console", out TaskContext c))
        {
            c.Changed -= action;
        }
    }

    private class TaskContext
    {
        public readonly List<RunningTask> Tasks = [];
        public event Action<ICommandStatus> Changed;

        public void FireChanged()
        {
            Changed?.Invoke(Tasks.LastOrDefault());
        }
    }

    private class RunningTask(string context, string maintext, string subtext) : ICommandStatus
    {
        private double? _progressValue;

        private double? _progressMax;

        private string _maintext = maintext;

        private string _subtext = subtext;

        private Exception _exception;

        private TaskStatus _status;

        public void DismissException()
        {
            if (_exception != null)
                Dispose();
        }

        public double? ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                var c = Tasks[context];
                c.FireChanged();
            }
        }

        public double? ProgressMax
        {
            get => _progressMax;
            set
            {
                _progressMax = value;
                var c = Tasks[context];
                c.FireChanged();
            }
        }

        public string Maintext
        {
            get => _maintext;
            set
            {
                _maintext = value;
                var c = Tasks[context];
                c.FireChanged();
            }
        }

        public string Subtext
        {
            get => _subtext;
            set
            {
                _subtext = value;
                var c = Tasks[context];
                c.FireChanged();
            }
        }

        public Exception Exception
        {
            get => _exception;
            set
            {
                _exception = value;
                var c = Tasks[context];
                c.FireChanged();
            }
        }

        public TaskStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                var c = Tasks[context];
                c.FireChanged();
            }
        }

        public void Dispose()
        {
            var c = Tasks[context];
            lock (c)
            {
                c.Tasks.Remove(this);
            }
            c.FireChanged();
        }
    }
}
