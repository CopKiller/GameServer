using System;
using Core.Service.Interfaces.Types;
using Game.Scripts.GameState.Interface;
using Godot;

namespace Game.Scripts.Singletons;

public partial class GameStateManager : Node
{
    private IGameStateBase? _currentState;
    
    private bool _inChangeState = false;
    
    public override void _Ready()
    {
        GD.Print("GameStateManager ready!");
    }
    
    public async void ChangeState<T>(IGameState<T> newState) where T : CanvasItem
    {
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
    }

    public void ExitGame()
    {
        GetTree().Quit();
    }

    public override void _ExitTree()
    {
        _currentState?.ExitStateAsync();
    }
}