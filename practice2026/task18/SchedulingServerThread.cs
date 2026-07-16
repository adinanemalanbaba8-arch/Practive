using System.Collections.Concurrent;

namespace Task18;

public class SchedulingServerThread
{
    private readonly BlockingCollection<Task17.ICommand> _incoming = new();
    private readonly IScheduler _scheduler;
    private readonly Thread _thread;
    private volatile bool _stopRequested;

    public SchedulingServerThread(IScheduler scheduler)
    {
        _scheduler = scheduler;
        _thread = new Thread(Run);
        _thread.IsBackground = true;
        _thread.Start();
    }

    public void Enqueue(Task17.ICommand command)
    {
        _incoming.Add(command);
    }

    public void Stop()
    {
        _stopRequested = true;
        _incoming.CompleteAdding();
    }

    private void Run()
    {
        while (!_stopRequested)
        {
            if (_scheduler.HasCommand())
            {
                if (_incoming.TryTake(out var newCommand, 0))
                {
                    Dispatch(newCommand);
                }

                var current = _scheduler.Select();

                try
                {
                    current.Execute();
                }
                catch (Exception)
                {
                    // gestion d exception facultative pour cette tache
                }

                if (!current.IsCompleted)
                {
                    _scheduler.Add(current);
                }
            }
            else
            {
                Task17.ICommand newCommand;

                try
                {
                    newCommand = _incoming.Take();
                }
                catch (InvalidOperationException)
                {
                    break;
                }

                Dispatch(newCommand);
            }
        }
    }

    private void Dispatch(Task17.ICommand command)
    {
        if (command is ILongCommand longCommand)
        {
            _scheduler.Add(longCommand);
        }
        else
        {
            try
            {
                command.Execute();
            }
            catch (Exception)
            {
                // gestion d exception facultative pour cette tache
            }
        }
    }

    public void Join()
    {
        _thread.Join();
    }

    public bool Join(TimeSpan timeout)
    {
        return _thread.Join(timeout);
    }
}
