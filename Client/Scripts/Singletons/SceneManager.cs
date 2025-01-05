using System;
using System.Reflection;
using System.Threading.Tasks;
using Game.Scripts.Cache;
using Game.Scripts.Extensions;
using Game.Scripts.Extensions.Attributes;
using Game.Scripts.Loader;
using Godot;

namespace Game.Scripts.Singletons;

public partial class SceneManager : Node
{
    private SceneTree? _sceneTree;
    private ScenePathCache? _scenePathCache;
    private Node? _currentScene;
    private CustomLoader? _customLoader;

    /// <summary>
    /// Emitted when a new scene is successfully loaded and assigned.
    /// </summary>
    [Signal]
    public delegate void SceneLoadedEventHandler(Node newScene);
    
    [Signal]
    public delegate void SceneChangedEventHandler(Node newScene);

    public Node? GetCurrentScene() => _currentScene;

    public override void _Ready()
    {
        GD.Print("SceneManager ready!");

        _sceneTree = GetTree();
        _scenePathCache = ServiceManager.GetRequiredService<ScenePathCache>();
        _customLoader = ServiceManager.GetRequiredService<CustomLoader>();

        _customLoader.Connect(CustomLoader.SignalName.LoadStarted, Callable.From<string>(OnSceneLoadStarted));
        _customLoader.Connect(CustomLoader.SignalName.LoadCompleted, Callable.From<string, Resource>(OnSceneLoadCompleted));
        _customLoader.Connect(CustomLoader.SignalName.LoadFailed, Callable.From<string, string>(OnSceneLoadFailed));
        _customLoader.Connect(CustomLoader.SignalName.LoadProgress, Callable.From<string, float>(OnSceneLoadProgress));
    }

    public override void _ExitTree()
    {
        _sceneTree = null;
        _currentScene = null;
    }

    public Task LoadSceneInBackground<T>(LoaderPriority priority) where T : Node
    {
        if (_scenePathCache == null || _sceneTree == null || _customLoader == null)
        {
            GD.PrintErr("SceneManager not ready!");
            return Task.CompletedTask;
        }

        var scenePath = _scenePathCache.GetScenePath<T>();

        LoadResource(scenePath, priority);
        
        return Task.CompletedTask;
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

    private void LoadResource(string path, LoaderPriority priority)
    {
        // Solicitar o carregamento do recurso
        GD.Print($"Starting to load resource: {path}");

        _customLoader?.EnqueueResource(path, priority); // Enfileirar o recurso
    }

    #region Signals

    private void OnSceneLoadStarted(string resourcePath)
    {
        // Reportar para o progresso de carregamento
        GD.Print($"Resource {resourcePath} started loading...");
    }
    
    private void OnSceneLoadCompleted(string resourcePath, Resource resource)
    {
        EmitSignal(SignalName.SceneLoaded, resource);
    }
    
    private void OnSceneChanged(Node newScene)
    {
        _currentScene = newScene;
        EmitSignal(SignalName.SceneChanged, newScene);
    }
    
    public void ChangeSceneToPacked(Resource sceneResource)
    {
        if (_sceneTree == null)
        {
            GD.PrintErr("SceneManager not ready!");
            return;
        }

        var sceneNode = (sceneResource as PackedScene)?.Instantiate();
        
        if (sceneNode == null)
        {
            GD.PrintErr("Failed to instantiate the scene!");
            return;
        }
        
        sceneNode.Connect(Node.SignalName.TreeEntered, Callable.From(() => OnSceneChanged(sceneNode)));
        
        _sceneTree.CurrentScene.QueueFree();
        _sceneTree.Root.AddChild(sceneNode);
        _sceneTree.CurrentScene = sceneNode;
    }
    
    private void OnSceneLoadFailed(string resourcePath, string error)
    {
        GD.PrintErr($"Resource Error to load {resourcePath} error: {error}");
    }
    
    private void OnSceneLoadProgress(string resourcePath, float progress)
    {
        // Reportar para o progresso de carregamento
        GD.Print($"Resource {resourcePath} loading progress: {progress:P}");
    }

    #endregion
}