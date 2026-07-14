using System;
using System.Linq;
using System.Reflection;

namespace task07;

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (displayNameAttr != null)
        {
            Console.WriteLine($"Отображаемое имя класса: {displayNameAttr.DisplayName}");
        }

        var versionAttr = type.GetCustomAttribute<VersionAttribute>();
        if (versionAttr != null)
        {
            Console.WriteLine($"Версия класса: {versionAttr}");
        }

        var members = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .Cast<MemberInfo>()
            .Concat(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

        foreach (var member in members)
        {
            var memberDisplayName = member.GetCustomAttribute<DisplayNameAttribute>();
            if (memberDisplayName != null)
            {
                Console.WriteLine($"{member.Name}: {memberDisplayName.DisplayName}");
                // test CI
            }
        }
    }
}
