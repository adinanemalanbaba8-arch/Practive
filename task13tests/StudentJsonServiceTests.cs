using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace task13tests;

public class StudentJsonServiceTests
{
    private readonly task13.StudentJsonService _service = new();

    private task13.Student CreateSampleStudent()
    {
        return new task13.Student
        {
            FirstName = "Иван",
            LastName = "Петров",
            BirthDate = new DateTime(2005, 3, 15),
            Grades = new List<task13.Subject>
            {
                new() { Name = "Математика", Grade = 5 },
                new() { Name = "Физика", Grade = 4 }
            }
        };
    }

    [Fact]
    public void Serialize_ReturnsJsonWithFormattedDate()
    {
        var student = CreateSampleStudent();
        var json = _service.Serialize(student);
        Assert.Contains("15.03.2005", json);
    }

    [Fact]
    public void Serialize_IgnoresNullGrades()
    {
        var student = CreateSampleStudent();
        student.Grades = null;
        var json = _service.Serialize(student);
        Assert.DoesNotContain("Grades", json);
    }

    [Fact]
    public void Deserialize_ReturnsCorrectStudent()
    {
        var student = CreateSampleStudent();
        var json = _service.Serialize(student);
        var result = _service.Deserialize(json);
        Assert.Equal(student.FirstName, result.FirstName);
        Assert.Equal(student.LastName, result.LastName);
        Assert.Equal(student.BirthDate, result.BirthDate);
        Assert.Equal(2, result.Grades!.Count);
    }

    [Fact]
    public void Deserialize_EmptyFirstName_ThrowsInvalidOperationException()
    {
        var student = CreateSampleStudent();
        student.FirstName = "";
        var json = _service.Serialize(student);
        Assert.Throws<InvalidOperationException>(() => _service.Deserialize(json));
    }

    [Fact]
    public void Deserialize_FutureBirthDate_ThrowsInvalidOperationException()
    {
        var student = CreateSampleStudent();
        student.BirthDate = DateTime.Now.AddYears(1);
        var json = _service.Serialize(student);
        Assert.Throws<InvalidOperationException>(() => _service.Deserialize(json));
    }

    [Fact]
    public void SaveToFile_ThenLoadFromFile_ReturnsSameStudent()
    {
        var student = CreateSampleStudent();
        var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        try
        {
            _service.SaveToFile(student, tempFile);
            var loaded = _service.LoadFromFile(tempFile);
            Assert.Equal(student.FirstName, loaded.FirstName);
            Assert.Equal(student.LastName, loaded.LastName);
            Assert.Equal(student.BirthDate, loaded.BirthDate);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public void LoadFromFile_NonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        Assert.Throws<FileNotFoundException>(() => _service.LoadFromFile(nonExistentPath));
    }
}
