using System.Collections.Concurrent;

namespace Task17;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _queue = new();
    private readonly Thread _thread;
    private volatile bool _hardStopRequested;

    public ServerThread()
    {
        _thread = new Thread(Run);
        _thread.IsBackground = true;
        _thread.Start();
    }

    public void Enqueue(ICommand command)
    {
        _queue.Add(command);
    }

    internal bool IsCurrentThread()
    {
        return Thread.CurrentThread == _thread;
    }

    internal void RequestHardStop()
    {
        _hardStopRequested = true;
        _queue.CompleteAdding();
    }

    internal void RequestSoftStop()
    {
        _queue.CompleteAdding();
    }

    private void Run()
    {
        try
        {
            foreach (var command in _queue.GetConsumingEnumerable())
            {
                if (_hardStopRequested)
                {
                    break;
                }

                try
                {
                    command.Execute();
                }
                catch (Exception)
                {
                    // ExceptionHandler facultatif pour cette tache (voir enonce)
                }
            }
        }
        catch (Exception)
        {
            // Securite : ne pas laisser le thread mourir silencieusement de facon inattendue
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
