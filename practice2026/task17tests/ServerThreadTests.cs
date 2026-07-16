using Xunit;

namespace Task17Tests;

public class ServerThreadTests
{
    private class ActionCommand : Task17.ICommand
    {
        private readonly Action _action;
        public ActionCommand(Action action) => _action = action;
        public void Execute() => _action();
    }

    [Fact]
    public void SoftStop_ExecutesAllQueuedCommandsBeforeStopping()
    {
        var server = new Task17.ServerThread();
        var executedCount = 0;
        var barrier = new ManualResetEventSlim(false);

        server.Enqueue(new ActionCommand(() => Interlocked.Increment(ref executedCount)));
        server.Enqueue(new ActionCommand(() => Interlocked.Increment(ref executedCount)));
        server.Enqueue(new ActionCommand(() => Interlocked.Increment(ref executedCount)));
        server.Enqueue(new Task17.SoftStopCommand(server));

        var completed = server.Join(TimeSpan.FromSeconds(2));

        Assert.True(completed);
        Assert.Equal(3, executedCount);
    }

    [Fact]
    public void HardStop_StopsImmediatelyIgnoringRemainingCommands()
    {
        var server = new Task17.ServerThread();
        var executedAfterStop = false;
        var readyToStop = new ManualResetEventSlim(false);

        server.Enqueue(new ActionCommand(() => readyToStop.Wait()));
        server.Enqueue(new Task17.HardStopCommand(server));
        server.Enqueue(new ActionCommand(() => executedAfterStop = true));

        readyToStop.Set();

        var completed = server.Join(TimeSpan.FromSeconds(2));

        Assert.True(completed);
        Assert.False(executedAfterStop);
    }

    [Fact]
    public void HardStop_FromOtherThread_ThrowsException()
    {
        var server = new Task17.ServerThread();
        var hardStop = new Task17.HardStopCommand(server);

        Assert.Throws<InvalidOperationException>(() => hardStop.Execute());

        server.Enqueue(new Task17.SoftStopCommand(server));
        server.Join(TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void SoftStop_FromOtherThread_ThrowsException()
    {
        var server = new Task17.ServerThread();
        var softStop = new Task17.SoftStopCommand(server);

        Assert.Throws<InvalidOperationException>(() => softStop.Execute());

        server.Enqueue(new Task17.SoftStopCommand(server));
        server.Join(TimeSpan.FromSeconds(2));
    }
}
