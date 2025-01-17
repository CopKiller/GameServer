using System;
using System.Threading;
using Core.Service.Interfaces.Types;
using Game.Scripts.GameState.Interface;
using Game.Scripts.Singletons.Interface;
using Godot;

namespace Game.Scripts.Singletons;

public partial class GameStateManager : Node, IGameStateManager
{
    private IGameStateBase? _currentState;
    
    private bool _inChangeState = false;
    
    private readonly SemaphoreSlim _stateLock = new(1, 1);
    
    public override void _Ready()
    {
        GD.Print("GameStateManager ready!");
    }
    
    public async void ChangeState<T>(IGameState<T> newState) where T : CanvasItem
    {
        await _stateLock.WaitAsync();
        
        try
        {
            if (_inChangeState)
            {
                GD.PrintErr("Already changing state!");
                return;
            }
            
            if (_currentState != null)
                await _currentState.ExitStateAsync();
            
            _currentState = newState;
            
            if (_currentState != null)
                await _currentState.EnterStateAsync();
            
            _inChangeState = false;
        }
        catch (Exception e)
        {
            GD.PrintErr($"Failed to change game state: {e.Message}");
        }
        finally
        {
            _stateLock.Release();
        }
    }
    
    public IGameStateBase? GetCurrentState()
    {
        return _currentState;
    }

    public void ExitGame()
    {
        GetTree().Quit();
    }

    public override void _ExitTree()
    {
        _currentState?.ExitStateAsync();
        
        _currentState = null;
    }
}