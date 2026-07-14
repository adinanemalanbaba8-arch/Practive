using System;
using System.Linq;
using System.Reflection;

namespace task09;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Использование: task09 <путь к DLL>");
            return;
        }

        var assemblyPath = args[0];
        var assembly = Assembly.LoadFrom(assemblyPath);

        var types = assembly.GetTypes().Where(t => t.IsClass);

        foreach (var type in types)
        {
            Console.WriteLine($"Класс: {type.FullName}");

            var classAttributes = type.GetCustomAttributes();
            foreach (var attr in classAttributes)
            {
                Console.WriteLine($"  Атрибут класса: {attr}");
            }

            var constructors = type.GetConstructors();
            foreach (var ctor in constructors)
            {
                var parameters = ctor.GetParameters()
                    .Select(p => $"{p.ParameterType.Name} {p.Name}");
                Console.WriteLine($"  Конструктор: ({string.Join(", ", parameters)})");
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName);
            foreach (var method in methods)
            {
                var parameters = method.GetParameters()
                    .Select(p => $"{p.ParameterType.Name} {p.Name}");
                Console.WriteLine($"  Метод: {method.ReturnType.Name} {method.Name}({string.Join(", ", parameters)})");

                var methodAttributes = method.GetCustomAttributes();
                foreach (var attr in methodAttributes)
                {
                    Console.WriteLine($"    Атрибут метода: {attr}");
                }
            }

            Console.WriteLine();
            // test CI
        }
    }
}
