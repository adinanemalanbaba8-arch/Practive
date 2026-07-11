using System;
using System.IO;
using task10;

namespace task10app;

class Program
{
    static void Main(string[] args)
    {
        var pluginsFolder = args.Length > 0 ? args[0] : AppContext.BaseDirectory;

        Console.WriteLine("Загрузка плагинов из папки: " + pluginsFolder);

        var loader = new PluginLoader();
        loader.LoadAndExecutePlugins(pluginsFolder);
    }
}
