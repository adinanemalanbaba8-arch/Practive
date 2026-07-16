namespace Task18;

public class RoundRobinScheduler : IScheduler
{
    private readonly Queue<ILongCommand> _commands = new();

    public bool HasCommand()
    {
        return _commands.Count > 0;
    }

    public ILongCommand Select()
    {
        return _commands.Dequeue();
    }

    public void Add(ILongCommand cmd)
    {
        _commands.Enqueue(cmd);
    }
}
