using System;
using task10;

namespace PluginA;

[PluginLoad]
public class PluginACommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Выполнение PluginA (без зависимостей)");
    }
}
