using System;
using System.IO;
using System.Linq;
using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _directoryPath;

    public DirectorySizeCommand(string directoryPath)
    {
        _directoryPath = directoryPath;
    }

    public void Execute()
    {
        var size = Directory.GetFiles(_directoryPath, "*", SearchOption.AllDirectories)
            .Sum(file => new FileInfo(file).Length);

        Console.WriteLine($"Размер каталога '{_directoryPath}': {size} байт");
    }
}
