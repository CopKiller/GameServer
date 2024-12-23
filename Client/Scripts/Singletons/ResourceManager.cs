using System;
using Core.Client.Resource;
using Godot;

namespace Game.Scripts.Singletons;

/// <summary>
/// Singleton responsável por gerenciar os recursos do jogo. 
/// </summary>
public partial class ResourceManager : Node
{
    private readonly ResourceManager<Resource> _resourceManager = new(new ResourceLoader());
    
    public override void _Ready()
    {
        ResourcePaths.LoadResourcesPath();
        LoadResources();
    }
    
    public void LoadResources()
    {
        // Scenes
        _resourceManager.LoadResource(EResourceType.Texture, EResource.SplashScreen);
        
        /*// Sprites
        _resourceManager.LoadResource(EResourceType.Texture, EResource.PlayerTexture);
        _resourceManager.LoadResource(EResourceType.Texture, EResource.EnemyTexture);
        
        // Sounds
        _resourceManager.LoadResource(EResourceType.Sound, EResource.ClickSound);
        _resourceManager.LoadResource(EResourceType.Sound, EResource.ExplosionSound);
        
        // Scripts
        _resourceManager.LoadResource(EResourceType.Script, EResource.PlayerScript);
        _resourceManager.LoadResource(EResourceType.Script, EResource.EnemyScript);*/
    }
    
    public T? GetResource<T>(EResourceType key, EResource resourceType) where T : Resource
    {
        return _resourceManager.LoadResource(key, resourceType) as T;
    }
    
    public bool UnloadResource(EResourceType key, EResource resourceType)
    {
        var resource = GetResource<Resource>(key, resourceType);
        
        if (resource == null) return false;
        
        return _resourceManager.UnloadResource(key, resourceType);
    }
    
    public void UnloadResource(EResourceType key)
    {
        _resourceManager.UnloadResource(key);
    }
    
    public void UnloadAllResources()
    {
        foreach (var key in Enum.GetValues<EResourceType>())
        {
            _resourceManager.UnloadResource(key);
        }
    }
}