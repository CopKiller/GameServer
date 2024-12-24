namespace Core.Client.Resource;

public static class ResourcePath
{
    private static readonly Dictionary<EResourceType, string> MainPaths = new();

    private static readonly Dictionary<EResource, (EResourceType type, string fileName)> ResourceMap = new();
    
    public static void LoadResourcesPath()
    {
        MainPaths[EResourceType.Scene] = "res://Client/Scenes/";
        MainPaths[EResourceType.Texture] = "res://Client/Resources/Texture/";
        MainPaths[EResourceType.Sound] = "res://Client/Resources/Sound/";
        MainPaths[EResourceType.Script] = "res://Client/Scripts/";
        
        PopulateResourceMap();
    }

    private static void PopulateResourceMap()
    {
        // Textures
        ResourceMap.Add(EResource.SplashScreen, (EResourceType.Texture, "SplashImage.jpg"));
        
        // Scenes
        ResourceMap.Add(EResource.LoadingScene, (EResourceType.Scene, "Transitions/LoadingScene.tscn"));
        ResourceMap.Add(EResource.LoadingScene, (EResourceType.Scene, "Transitions/SplashScene.tscn"));
    }

    public static string GetResourcePath(EResource resource)
    {
        if (ResourceMap.TryGetValue(resource, out var resourceInfo))
        {
            return MainPaths[resourceInfo.type] + resourceInfo.fileName;
        }

        //.PrintErr($"Resource {resource} not found!");
        return string.Empty;
    }

    public static string GetScenePath(EResource resource)
    {
        return GetResourcePathByType(resource, EResourceType.Scene);
    }

    public static string GetTexturePath(EResource resource)
    {
        return GetResourcePathByType(resource, EResourceType.Texture);
    }

    private static string GetResourcePathByType(EResource resource, EResourceType type)
    {
        if (ResourceMap.TryGetValue(resource, out var resourceInfo) && resourceInfo.type == type)
        {
            return MainPaths[resourceInfo.type] + resourceInfo.fileName;
        }

        //GD.PrintErr($"Resource {resource} is not of type {type}!");
        return string.Empty;
    }
}