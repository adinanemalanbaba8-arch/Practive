using Xunit;

namespace Task18Tests;

public class RoundRobinSchedulerTests
{
    private class CountingLongCommand : Task18.ILongCommand
    {
        private readonly int _stepsNeeded;
        private int _stepsDone;
        public List<int> ExecutionOrder { get; }
        public int Id { get; }

        public CountingLongCommand(int id, int stepsNeeded, List<int> sharedLog)
        {
            Id = id;
            _stepsNeeded = stepsNeeded;
            ExecutionOrder = sharedLog;
        }

        public bool IsCompleted => _stepsDone >= _stepsNeeded;

        public void Execute()
        {
            _stepsDone++;
            ExecutionOrder.Add(Id);
        }
    }

    [Fact]
    public void LongCommand_RequiresMultipleExecuteCallsToComplete()
    {
        var log = new List<int>();
        var command = new CountingLongCommand(1, stepsNeeded: 3, log);

        command.Execute();
        Assert.False(command.IsCompleted);

        command.Execute();
        Assert.False(command.IsCompleted);

        command.Execute();
        Assert.True(command.IsCompleted);
    }

    [Fact]
    public void Scheduler_AlternatesBetweenCommands_RoundRobin()
    {
        var log = new List<int>();
        var scheduler = new Task18.RoundRobinScheduler();
        var commandA = new CountingLongCommand(1, stepsNeeded: 3, log);
        var commandB = new CountingLongCommand(2, stepsNeeded: 3, log);

        scheduler.Add(commandA);
        scheduler.Add(commandB);

        while (scheduler.HasCommand())
        {
            var current = scheduler.Select();
            current.Execute();
            if (!current.IsCompleted)
            {
                scheduler.Add(current);
            }
        }

        Assert.Equal(new[] { 1, 2, 1, 2, 1, 2 }, log);
    }

    [Fact]
    public void SchedulingServerThread_CompletesLongCommandAcrossMultipleTicks()
    {
        var log = new List<int>();
        var scheduler = new Task18.RoundRobinScheduler();
        var server = new Task18.SchedulingServerThread(scheduler);
        var command = new CountingLongCommand(1, stepsNeeded: 5, log);

        server.Enqueue(command);

        var deadline = DateTime.UtcNow.AddSeconds(2);
        while (!command.IsCompleted && DateTime.UtcNow < deadline)
        {
            Thread.Sleep(20);
        }

        Assert.True(command.IsCompleted);
        server.Stop();
        server.Join(TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void SchedulingServerThread_DoesNotDeadlock_WhenAddingWhileLongCommandsRunning()
    {
        var log = new List<int>();
        var scheduler = new Task18.RoundRobinScheduler();
        var server = new Task18.SchedulingServerThread(scheduler);
        var longCommand = new CountingLongCommand(1, stepsNeeded: 50, log);

        server.Enqueue(longCommand);
        Thread.Sleep(50);

        var secondExecuted = false;
        var secondCommand = new CountingLongCommand(2, stepsNeeded: 1, log);
        server.Enqueue(secondCommand);

        var deadline = DateTime.UtcNow.AddSeconds(2);
        while (!secondCommand.IsCompleted && DateTime.UtcNow < deadline)
        {
            Thread.Sleep(20);
        }

        secondExecuted = secondCommand.IsCompleted;

        Assert.True(secondExecuted);
        server.Stop();
        server.Join(TimeSpan.FromSeconds(2));
    }
}
