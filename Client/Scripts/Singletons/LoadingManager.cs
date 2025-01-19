using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.BaseControls;
using Game.Scripts.Extensions;
using Godot;

namespace Game.Scripts.Singletons;

public partial class LoadingManager : Node
{
    private PackedScene? _loadingPackedScene;
    
    private LoadingScript? _loadingScene;
    
    private SceneManager? _sceneManager;

    private readonly Queue<(Func<Task>, string)> _tasks = new();
    private bool _isLoading;
    
    public bool IsLoading => _isLoading;

    [Signal]
    public delegate void LoadingCompleteEventHandler();

    public void AddTask(Func<Task> task, string description) => _tasks.Enqueue((task, description));

    public override void _Ready()
    {
        GD.Print("LoadingManager ready!");
        
        _sceneManager = ServiceManager.GetRequiredService<SceneManager>();
        _loadingPackedScene = _sceneManager.LoadScenePacked<LoadingScript>();
        _sceneManager.Connect(SceneManager.SignalName.SceneLoaded, Callable.From<Resource>(OnSceneLoaded));
    }

    public async Task StartLoading()
    {
        if (_isLoading) return;
        _isLoading = true;

        if (_loadingPackedScene == null)
        {
            GD.PrintErr("Loading scene not found!");
            return;
        }

        var progress = 0;
        var totalTasks = _tasks.Count;

        if (totalTasks == 0)
        {
            GD.Print("Nenhuma tarefa para carregar!");
            return;
        }

        _loadingScene = _loadingPackedScene.Instantiate<LoadingScript>();
        await AddChildAsync(_loadingScene);
        await _loadingScene.FadeIn();

        while (_tasks.Count > 0)
        {
            var (task, description) = _tasks.Dequeue();
            try
            {
                GD.Print($"Iniciando: {description}");
                _loadingScene.UpdateMessage(description);
                await task();
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erro: {description} - {ex.Message}");
            }

            progress++;
            await _loadingScene.SmoothUpdateProgressBar(progress / (float)totalTasks, 0.5f);
        }
    }
    
    private async void OnSceneLoaded(Resource newScene)
    {
        if (!_isLoading) return;
        
        if (_loadingScene is null)
        {
            GD.PrintErr("Loading scene not found!");
            return;
        }
        
        _loadingScene.UpdateMessage("Carregamento completo!");

        await Task.Delay(100);

        await _loadingScene.FadeOut(0.5f);

        EmitSignal(SignalName.LoadingComplete);

        ResetLoading();
        
        _sceneManager?.ChangeSceneToPacked(newScene);
    }

    private async Task AddChildAsync(Node child)
    {
        GetTree().Root.CallDeferred(Node.MethodName.AddChild, child);
        
        await ToSignal(child, Node.SignalName.Ready);
    }

    public void ResetLoading()
    {
        _loadingScene?.QueueFree();
        _isLoading = false;
    }
}