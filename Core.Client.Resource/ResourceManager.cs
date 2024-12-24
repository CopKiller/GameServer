using Core.Client.Resource.Interface;

namespace Core.Client.Resource;

public class ResourceManager<T> where T : class
{
    /// <summary>
    /// Cache de recursos carregados.
    /// </summary>
    private readonly Dictionary<EResourceType, ResourcesUploaded<T>> _resourceCache = new();
    
    private readonly IResourceLoader _resourceLoader;
    
    public ResourceManager(IResourceLoader loader)
    {
        _resourceLoader = loader;
        
        foreach (var key in Enum.GetValues<EResourceType>())
        {
            _resourceCache.Add(key, new ResourcesUploaded<T>());
        }
    }
    
    public T? LoadResource(EResourceType key, EResource resourceType)
    {
        if (_resourceCache.TryGetValue(key, out var resourcesUploaded))
        {
            if (resourcesUploaded.GetResource(resourceType) is { } res)
            {
                //GD.Print($"[{nameof(ResourceManager<T>)}] Recurso carregado do cache: {resourceType.GetType().Name}");
                return res;
            }
        }
        
        var path = ResourcePath.GetResourcePath(resourceType);
                
        T? resource = _resourceLoader.Load<T>(path);
        if (resource != null)
        {
            _resourceCache[key].AddResource(resourceType, resource);
            //GD.Print($"[{nameof(ResourceManager<T>)}] Recurso carregado e cacheado: {path}");
        }
        else
        {
            //GD.PrintErr($"[{nameof(ResourceManager<T>)}] Falha ao carregar recurso: {path}");
        }

        return resource;
    }
    
    public bool UnloadResource(EResourceType key, EResource resourceType)
    {
        if (!_resourceCache.TryGetValue(key, out var resource)) return false;
        
        return resource.RemoveResource(resourceType);
    }

    public void UnloadResource(EResourceType key)
    {
        if (!_resourceCache.TryGetValue(key, out var resource)) return;

        resource?.RemoveResources();
    }

    /// <summary>
    /// Descarta todos os recursos carregados.
    /// </summary>
    public void UnloadResources()
    {
        foreach (var resource in _resourceCache.Values)
        {
            resource?.RemoveResources();
        }

        _resourceCache.Clear();
        //GD.Print($"[{nameof(ResourceManager<T>)}] Todos os recursos foram descarregados.");
    }
}