using Task17;
using Task18;
using Task19;

var scheduler = new RoundRobinScheduler();
var server = new SchedulingServerThread(scheduler);

var commands = new List<TestCommand>();
for (int i = 1; i <= 5; i++)
{
    var cmd = new TestCommand(i);
    commands.Add(cmd);
    server.Enqueue(cmd);
}

while (commands.Exists(c => !c.IsCompleted))
{
    Thread.Sleep(20);
}

Console.WriteLine("Toutes les commandes ont termine leurs 3 executions. Arret du thread (HardStop).");

server.Stop();
server.Join(TimeSpan.FromSeconds(2));

Console.WriteLine("Thread arrete.");
