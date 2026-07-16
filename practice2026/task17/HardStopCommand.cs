namespace Task17;

public class HardStopCommand : ICommand
{
    private readonly ServerThread _target;

    public HardStopCommand(ServerThread target)
    {
        _target = target;
    }

    public void Execute()
    {
        if (!_target.IsCurrentThread())
        {
            throw new InvalidOperationException("HardStop ne peut etre execute que par le thread cible.");
        }

        _target.RequestHardStop();
    }
}
