using Core.Client.Resource.Interface;
using Godot;

namespace Game.Scripts.Resources;

public class ResourceLoader : IResourceLoader
{
    public T? Load<T>(string path) where T : class
    {
        return GD.Load<T>(path);
    }
}