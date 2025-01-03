using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Extensions;
using Game.Scripts.Transitions;
using Godot;

namespace Game.Scripts.Singletons;

public partial class LoadingManager : Node
{
    private PackedScene? _loadingPackedScene;
    
    private LoadingScript? _loadingScene;

    private readonly Queue<(Func<Task>, string)> _tasks = new();
    private bool _isLoading;

    [Signal]
    public delegate void LoadingCompleteEventHandler();

    public void AddTask(Func<Task> task, string description) => _tasks.Enqueue((task, description));

    public override void _Ready()
    {
        GD.Print("LoadingManager ready!");
        
        var sceneManager = ServiceManager.GetRequiredService<SceneManager>();
        _loadingPackedScene = sceneManager.LoadScenePacked<LoadingScript>();
    }

    public override void _ExitTree()
    {
        ResetLoading();
    }

    public async Task StartLoading(Func<Task>? nextSceneTask = null)
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
        
        _loadingScene.UpdateMessage("Carregamento completo!");
        await _loadingScene.SmoothUpdateProgressBar(progress / (float)totalTasks, 0.5f);

        await _loadingScene.FadeOut(0.5f);
        
        if (nextSceneTask != null)
            await nextSceneTask();

        EmitSignal(SignalName.LoadingComplete);

        ResetLoading();
    }

    private Task AddChildAsync(Node child)
    {
        var tcs = new TaskCompletionSource();
        
        child.Connect(Node.SignalName.Ready, Callable.From(OnReady));
        
        GetTree().Root.CallDeferred(Node.MethodName.AddChild, child);
        
        return tcs.Task;
        
        void OnReady()
        {
            tcs.TrySetResult();
        }
    }

    private void ResetLoading()
    {
        _isLoading = false;
        _loadingScene?.QueueFree();
    }
}