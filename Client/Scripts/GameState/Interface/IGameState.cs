using System.Threading.Tasks;
using Godot;

namespace Game.Scripts.GameState.Interface;

/// <summary>
/// Interface para implementar um estado do jogo.
/// </summary>
public interface IGameState<T> : IGameStateBase where T : CanvasItem
{
    
}
