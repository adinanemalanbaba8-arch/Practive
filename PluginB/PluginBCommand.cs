using System;
using task10;

namespace PluginB;

[PluginLoad("PluginACommand")]
public class PluginBCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Выполнение PluginB (зависит от PluginA)");
    }
}
