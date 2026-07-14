using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace task10;

public class PluginLoader
{
    public void LoadAndExecutePlugins(string folderPath)
    {
        var dllFiles = Directory.GetFiles(folderPath, "*.dll");

        var pluginTypes = new List<Type>();
        foreach (var dllFile in dllFiles)
        {
            var assembly = Assembly.LoadFrom(dllFile);
            var typesInAssembly = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<PluginLoadAttribute>() != null && typeof(ICommand).IsAssignableFrom(t));
            pluginTypes.AddRange(typesInAssembly);
        }

        var orderedTypes = TopologicalSort(pluginTypes);

        foreach (var type in orderedTypes)
        {
            object created = Activator.CreateInstance(type);
            var instance = (ICommand)created;
            instance.Execute();
        }
    }

    public List<Type> TopologicalSort(List<Type> types)
    {
        var typesByName = types.ToDictionary(t => t.Name, t => t);
        var visited = new HashSet<string>();
        var visiting = new HashSet<string>();
        var result = new List<Type>();

        void Visit(Type type)
        {
            if (visited.Contains(type.Name))
            {
                return;
            }

            if (visiting.Contains(type.Name))
            {
                throw new InvalidOperationException("Обнаружена циклическая зависимость плагинов на классе " + type.Name);
            }

            visiting.Add(type.Name);

            var attribute = type.GetCustomAttribute<PluginLoadAttribute>();
            var neighbors = attribute != null ? attribute.Dependencies : Array.Empty<string>();

            foreach (var neighborName in neighbors)
            {
                if (typesByName.TryGetValue(neighborName, out var neighborType))
                {
                    Visit(neighborType);
                }
            }

            visiting.Remove(type.Name);
            visited.Add(type.Name);
            result.Add(type);
        }

        foreach (var type in types)
        {
            Visit(type);
        }

        return result;
        // test CI
    }
}
