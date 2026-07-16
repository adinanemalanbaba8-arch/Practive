namespace Task19;

public class TestCommand : Task18.ILongCommand
{
    private readonly int _id;
    private int _counter;

    public TestCommand(int id)
    {
        _id = id;
    }

    public bool IsCompleted => _counter >= 3;

    public void Execute()
    {
        _counter++;
        Console.WriteLine($"Potok {_id} vyzov {_counter}");
    }
}
