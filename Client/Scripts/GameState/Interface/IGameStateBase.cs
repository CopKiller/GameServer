using System.Threading.Tasks;

namespace Game.Scripts.GameState.Interface;

public interface IGameStateBase
{
    /// <summary>
    /// Configura o fade in.
    /// </summary>
    /// <param name="fadeIn">Efeito de cena surgindo</param>
    /// <param name="duration">Duração do efeito</param>
    void SetFadeIn(bool fadeIn, float duration = 0.5f);
    
    /// <summary>
    /// Configura o fade out.
    /// </summary>
    /// <param name="fadeOut">Efeito da cena desaparecendo</param>
    /// <param name="duration">Duração do efeito</param>
    void SetFadeOut(bool fadeOut, float duration = 0.5f);
    
    /// <summary>
    /// Entra no estado.
    /// </summary>
    /// <param name="node">Caso seja o primeiro nó a ser executado, configura o estado para ele.</param>
    Task EnterStateAsync();
    
    /// <summary>
    /// Sai do estado.
    /// </summary>
    Task ExitStateAsync();
}