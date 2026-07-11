using Xunit;
using System.Reflection;

namespace task07tests;

public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(task07.SampleClass);
        var attribute = type.GetCustomAttribute<task07.DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(task07.SampleClass).GetMethod("TestMethod");
        var attribute = method.GetCustomAttribute<task07.DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(task07.SampleClass).GetProperty("Number");
        var attribute = prop.GetCustomAttribute<task07.DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(task07.SampleClass);
        var attribute = type.GetCustomAttribute<task07.VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void PrintTypeInfo_DoesNotThrow()
    {
        var exception = Record.Exception(() => task07.ReflectionHelper.PrintTypeInfo(typeof(task07.SampleClass)));
        Assert.Null(exception);
    }
}
