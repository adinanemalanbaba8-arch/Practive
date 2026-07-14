using System;
using System.IO;
using System.Text.Json;

namespace task13;

public class StudentJsonService
{
    private readonly JsonSerializerOptions _options;

    public StudentJsonService()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            Converters = { new DateOnlyJsonConverter() }
        };
    }

    public string Serialize(Student student)
    {
        return JsonSerializer.Serialize(student, _options);
    }

    public Student Deserialize(string json)
    {
        var student = JsonSerializer.Deserialize<Student>(json, _options);

        if (student == null)
        {
            throw new JsonException("Не удалось десериализовать объект Student: результат null.");
        }

        Validate(student);

        return student;
    }

    public void SaveToFile(Student student, string filePath)
    {
        var json = Serialize(student);
        File.WriteAllText(filePath, json);
    }

    public Student LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Файл не найден.", filePath);
        }

        var json = File.ReadAllText(filePath);
        return Deserialize(json);
    }

    private void Validate(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.FirstName))
        {
            throw new InvalidOperationException("Имя студента не может быть пустым.");
        }

        if (string.IsNullOrWhiteSpace(student.LastName))
        {
            throw new InvalidOperationException("Фамилия студента не может быть пустой.");
        }

        if (student.BirthDate > DateTime.Now)
        {
            throw new InvalidOperationException("Дата рождения не может быть в будущем.");
        }
    }
}
