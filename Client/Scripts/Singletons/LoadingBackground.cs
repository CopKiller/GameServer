using Game.Scripts.Loader;
using Godot;

namespace Game.Scripts.Singletons;

public partial class LoadingBackground : Node
{
    private ILoaderService? _loaderService;
    
    [Signal]
    public delegate void LoadStartedEventHandler(string resourcePath);

    [Signal]
    public delegate void LoadProgressEventHandler(string resourcePath, float progress);

    [Signal]
    public delegate void LoadCompletedEventHandler(string resourcePath, Resource resource);

    [Signal]
    public delegate void LoadFailedEventHandler(string resourcePath, string error);

    public override void _Ready()
    {
        GD.Print("CustomLoader ready!");
        
        _loaderService = ServiceManager.GetRequiredService<ILoaderService>();
        _loaderService.LoadStarted += OnLoadStarted;
        _loaderService.LoadProgress += OnLoadProgress;
        _loaderService.LoadCompleted += OnLoadCompleted;
        _loaderService.LoadFailed += OnLoadFailed;
    }

    public override void _Process(double delta)
    {
        _loaderService?.ProcessQueue();
    }

    public void EnqueueResource(string resourcePath, LoaderPriority priority)
    {
        _loaderService?.EnqueueResource(resourcePath, priority);

    }

    private void OnLoadStarted(string resourcePath)
    {
        EmitSignal(SignalName.LoadStarted, resourcePath);
    }

    private void OnLoadProgress(string resourcePath, float progress)
    {
        EmitSignal(SignalName.LoadProgress, resourcePath, progress);
    }

    private void OnLoadCompleted(string resourcePath, Resource resource)
    {
        EmitSignal(SignalName.LoadCompleted, resourcePath, resource);
    }

    private void OnLoadFailed(string resourcePath, string error)
    {
        EmitSignal(SignalName.LoadFailed, resourcePath, error);
    }
}