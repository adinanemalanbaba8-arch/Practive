using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace task05;

public class ClassAnalyzer
{
    private readonly Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    // Список публичных методов
    public IEnumerable<string> GetPublicMethods()
        => _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .Select(m => m.Name);

    // Список имен параметров и возвращаемого значения публичного метода
    public IEnumerable<string> GetMethodParams(string methodname)
    {
        var method = _type.GetMethod(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        return (method?.GetParameters().Select(p => p.Name!) ?? Enumerable.Empty<string>())
            .Append(method?.ReturnType.Name ?? string.Empty);
    }

    // Список имен полей (включая приватные)
    public IEnumerable<string> GetAllFields()
        => _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(f => f.Name);

    // Список имен свойств
    public IEnumerable<string> GetProperties()
        => _type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(p => p.Name);

    // Наличие атрибута указанного типа у класса
    public bool HasAttribute<T>() where T : Attribute
        => _type.GetCustomAttribute<T>() != null;
}
