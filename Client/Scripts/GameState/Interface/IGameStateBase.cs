namespace Game.Scripts.GameState.Interface;

public interface IGameStateBase
{
    /// <summary>
    /// Entra no estado.
    /// </summary>
    /// <param name="node">Caso seja o primeiro nó a ser executado, configura o estado para ele.</param>
    void EnterState();
    
    /// <summary>
    /// Sai do estado.
    /// </summary>
    void ExitState();
}