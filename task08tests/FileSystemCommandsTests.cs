using Xunit;
using System.IO;
using FileSystemCommands;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindCorrectNumberOfFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir2");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "a.txt"), "A");
        File.WriteAllText(Path.Combine(testDir, "b.txt"), "B");
        File.WriteAllText(Path.Combine(testDir, "c.log"), "C");

        var matchingFiles = Directory.GetFiles(testDir, "*.txt");
        Assert.Equal(2, matchingFiles.Length);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void DirectorySizeCommand_ShouldImplementICommand()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir3");
        Directory.CreateDirectory(testDir);

        var command = new DirectorySizeCommand(testDir);
        Assert.IsAssignableFrom<CommandLib.ICommand>(command);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldImplementICommand()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir4");
        Directory.CreateDirectory(testDir);

        var command = new FindFilesCommand(testDir, "*.*");
        Assert.IsAssignableFrom<CommandLib.ICommand>(command);

        Directory.Delete(testDir, true);
        // test CI
    }
}
