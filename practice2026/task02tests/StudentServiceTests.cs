using NUnit.Framework;
using task02;

namespace task02tests;

public class StudentServiceTests
{
    private List<Student> _testStudents;
    private StudentService _service;

    [SetUp]
    public void Setup()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Test]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(s => s.Faculty == "ФИТ"), Is.True);
    }

    [Test]
    public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsWithMinAverageGrade(4.5).ToList();
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Select(s => s.Name), Does.Contain("Иван"));
        Assert.That(result.Select(s => s.Name), Does.Contain("Петр"));
        Assert.That(result.Select(s => s.Name), Does.Not.Contain("Анна"));
    }

    [Test]
    public void GetStudentsOrderedByName_ReturnsSortedList()
    {
        var result = _service.GetStudentsOrderedByName().ToList();
        var names = result.Select(s => s.Name).ToList();
        Assert.That(names, Is.EqualTo(new[] { "Анна", "Иван", "Петр" }));
    }

    [Test]
    public void GroupStudentsByFaculty_GroupsCorrectly()
    {
        var result = _service.GroupStudentsByFaculty();
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result["ФИТ"].Count(), Is.EqualTo(2));
        Assert.That(result["Экономика"].Count(), Is.EqualTo(1));
    }

    [Test]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.That(result, Is.EqualTo("Экономика"));
    }
}
