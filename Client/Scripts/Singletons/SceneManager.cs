using System;
using System.Reflection;
using System.Threading.Tasks;
using Game.Scripts.Cache;
using Game.Scripts.Extensions;
using Game.Scripts.Extensions.Attributes;
using Godot;

namespace Game.Scripts.Singletons;

public partial class SceneManager : Node
{
    private SceneTree? _sceneTree;
    private ScenePathCache? _scenePathCache;
    private CanvasItem? _currentScene;
    
    /// <summary>
    /// Emitted when a new scene is successfully loaded and assigned.
    /// </summary>
    [Signal]
    public delegate void SceneLoadedEventHandler(CanvasItem newScene);

    public override void _Ready()
    {
        GD.Print("SceneManager ready!");
        
        _sceneTree = GetTree();
        _scenePathCache = ServiceManager.GetRequiredService<ScenePathCache>();
    }

    public override void _ExitTree()
    {
        _sceneTree = null;
        _currentScene = null;
    }

    public bool LoadSceneInBackground<T>() where T : Node
    {
        if (_scenePathCache == null)
        {
            GD.PrintErr("SceneManager not ready!");
            return false;
        }
        
        var scenePath = _scenePathCache.GetScenePath<T>();
        
        return LoadResource(scenePath);
    }
    
    /// <summary>
    /// Changes the currently loaded scene in the background with optional fade transitions.
    /// </summary>
    /// <typeparam name="T">The type of the scene to load.</typeparam>
    /// <param name="fadeout">Whether to fade out the current scene.</param>
    /// <param name="fadeIn">Whether to fade in the new scene.</param>
    /// <param name="duration">Duration of the fade effects.</param>
    public async Task ChangeSceneLoadedInBackground<T>() where T : CanvasItem
    {
        if (_sceneTree == null || _scenePathCache == null)
        {
            GD.PrintErr("SceneManager not ready!");
            return;
        }
        
        var scenePath = _scenePathCache.GetScenePath<T>();
        
        while (ResourceLoader.LoadThreadedGetStatus(scenePath) == ResourceLoader.ThreadLoadStatus.InProgress)
        {
            await Task.Delay(100); // Aguarde 100ms entre verificações
        }
        
        var status = ResourceLoader.LoadThreadedGetStatus(scenePath);
        if (status == ResourceLoader.ThreadLoadStatus.Loaded)
        {
            var packedScene = ResourceLoader.LoadThreadedGet(scenePath) as PackedScene;
            
            _sceneTree.ChangeSceneToPacked(packedScene);
            await ToSignal(_sceneTree, SceneTree.SignalName.TreeChanged);

            // Assign and validate the new scene
            var scene = _sceneTree.CurrentScene as T;
            if (scene == null)
            {
                GD.PrintErr($"Scene {scenePath} is not of type {typeof(T).Name}!");
                return;
            }

            _currentScene = scene;
            
            // Emit the scene-loaded signal
            EmitSignal(SignalName.SceneLoaded, _currentScene);
        }
        else
        {
            GD.PrintErr($"Failed to load resource. Status: {status}");
            // Carregar agora!
            LoadResource(scenePath);
        }
    }
    
    public T? LoadSceneInstance<T>() where T : Node
    {
        if (_scenePathCache == null)
        {
            GD.PrintErr("SceneManager not ready!");
            return null;
        }
        
        var scenePath = _scenePathCache.GetScenePath<T>();

        return ResourceLoader.Load<PackedScene>(scenePath)?.Instantiate<T>();
    }
    
    public PackedScene? LoadScenePacked<T>() where T : Node
    {
        if (_scenePathCache == null)
        {
            GD.PrintErr("SceneManager not ready!");
            return null;
        }
        
        var scenePath = _scenePathCache.GetScenePath<T>();

        return ResourceLoader.Load<PackedScene>(scenePath);
    }
    
    
    private bool LoadResource(string path)
    {
        // Solicitar o carregamento do recurso
        GD.Print($"Starting to load resource: {path}");
        var requestId = ResourceLoader.LoadThreadedRequest(path);

        if (requestId == Error.Ok)
        {
            return true;
        }
        
        GD.PrintErr("Failed to start loading resource.");
        return false;
    }

    public Node? GetCurrentScene() => _currentScene;
}
