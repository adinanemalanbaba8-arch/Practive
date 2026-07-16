namespace Task18;

public interface ILongCommand : Task17.ICommand
{
    bool IsCompleted { get; }
}
