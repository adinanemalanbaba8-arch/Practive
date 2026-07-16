namespace Task17;

public class SoftStopCommand : ICommand
{
    private readonly ServerThread _target;

    public SoftStopCommand(ServerThread target)
    {
        _target = target;
    }

    public void Execute()
    {
        if (!_target.IsCurrentThread())
        {
            throw new InvalidOperationException("SoftStop ne peut etre execute que par le thread cible.");
        }

        _target.RequestSoftStop();
    }
}
