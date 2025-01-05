using Game.Scripts.GameState.Interface;
using Godot;

namespace Game.Scripts.Singletons.Interface;

public interface IGameStateManager
{
    void ChangeState<T>(IGameState<T> newState) where T : CanvasItem;
    
    IGameStateBase? GetCurrentState();
    
    void ExitGame();
}