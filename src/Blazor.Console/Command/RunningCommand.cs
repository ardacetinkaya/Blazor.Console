using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Console.Command
{
    public class RunningCommand : IRunningCommand
    {
        private static ConcurrentDictionary<string, TaskContext> _tasks = new ConcurrentDictionary<string, TaskContext>();

        public async Task StartCommandAsync(Func<ICommandStatus, Task> action, string maintext = null, string subtext = null)
        {
            var task = new RunningTask("console", maintext, subtext);
            if (!_tasks.TryGetValue("console", out TaskContext c))
            {
                c = new TaskContext
                {
                    Tasks = { task }
                };
                _tasks.TryAdd("console", c);
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
            if (!_tasks.TryGetValue("console", out TaskContext c))
            {
                c = new TaskContext();
                _tasks.TryAdd("console", c);
            }

            c.Changed += action;
            c.FireChanged();
        }

        public void UnsubscribeFromCommandProgressChanged(Action<ICommandStatus> action)
        {
            if (_tasks.TryGetValue("console", out TaskContext c))
            {
                c.Changed -= action;
            }
        }

        private class TaskContext
        {
            public List<RunningTask> Tasks = new List<RunningTask>();
            public event Action<ICommandStatus> Changed;

            public void FireChanged()
            {
                Changed?.Invoke(Tasks.LastOrDefault());
            }
        }

        private class RunningTask : ICommandStatus
        {
            private string _context;

            private double? _progressValue;

            private double? _progressMax;

            private string _maintext;

            private string _subtext;

            private Exception _exception;

            private TaskStatus _status;

            public RunningTask(string context, string maintext, string subtext)
            {
                _context = context;
                _maintext = maintext;
                _subtext = subtext;
            }

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
                    var c = _tasks[_context];
                    c.FireChanged();
                }
            }

            public double? ProgressMax
            {
                get => _progressMax;
                set
                {
                    _progressMax = value;
                    var c = _tasks[_context];
                    c.FireChanged();
                }
            }

            public string Maintext
            {
                get => _maintext;
                set
                {
                    _maintext = value;
                    var c = _tasks[_context];
                    c.FireChanged();
                }
            }

            public string Subtext
            {
                get => _subtext;
                set
                {
                    _subtext = value;
                    var c = _tasks[_context];
                    c.FireChanged();
                }
            }

            public Exception Exception
            {
                get => _exception;
                set
                {
                    _exception = value;
                    var c = _tasks[_context];
                    c.FireChanged();
                }
            }

            public TaskStatus Status
            {
                get => _status;
                set
                {
                    _status = value;
                    var c = _tasks[_context];
                    c.FireChanged();
                }
            }

            public void Dispose()
            {
                var c = _tasks[_context];
                lock (c)
                {
                    c.Tasks.Remove(this);
                }
                c.FireChanged();
            }
        }
    }



}
