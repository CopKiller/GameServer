using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Game.Scripts.Extensions.Attributes;

namespace Game.Scripts.Cache;

public class ScenePathCache
{
    private readonly Dictionary<Type, string> _pathCache = new();
    
    public ScenePathCache()
    {
        InitializeCache();
    }

    private void InitializeCache()
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        var typesWithAttribute = assembly.GetTypes()
            .Where(type => type.GetCustomAttribute<ScenePathAttribute>() != null);

        foreach (var type in typesWithAttribute)
        {
            var attribute = type.GetCustomAttribute<ScenePathAttribute>();
            if (attribute?.Path != null)
            {
                _pathCache[type] = attribute.Path;
            }
        }
    }

    public string GetScenePath<T>()
    {
        var type = typeof(T);

        if (_pathCache.TryGetValue(type, out var cachedPath))
        {
            return cachedPath;
        }

        throw new KeyNotFoundException($"Scene path not found for {type.Name}!");
    }
}