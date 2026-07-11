using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLib;

namespace CommandRunner;

class Program
{
    static void Main(string[] args)
    {
        var testDir = Path.Combine(Path.GetTempPath(), "CommandRunnerDemo");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "sample1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "sample2.log"), "World");

        var assemblyPath = Path.Combine(AppContext.BaseDirectory, "FileSystemCommands.dll");
        var assembly = Assembly.LoadFrom(assemblyPath);

        var commandTypes = assembly.GetTypes()
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in commandTypes)
        {
            ICommand? command = type.Name switch
            {
                "DirectorySizeCommand" => (ICommand)Activator.CreateInstance(type, testDir)!,
                "FindFilesCommand" => (ICommand)Activator.CreateInstance(type, testDir, "*.txt")!,
                _ => null
            };

            command?.Execute();
        }

        Directory.Delete(testDir, true);
    }
}
