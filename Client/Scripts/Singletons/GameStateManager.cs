using System;
using Core.Service.Interfaces.Types;
using Game.Scripts.GameState.Interface;
using Godot;

namespace Game.Scripts.Singletons;

public partial class GameStateManager : Node
{
    private IGameStateBase? _currentState;
    
    public override void _Ready()
    {
        GD.Print("GameStateManager ready!");
    }
    
    public void ChangeState<T>(IGameState<T> newState) where T : CanvasItem
    {
        try
        {
            _currentState?.ExitState();

            _currentState = newState;
            _currentState.EnterState();
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
        _currentState?.ExitState();
    }
}