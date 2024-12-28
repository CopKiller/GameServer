using System;
using System.Threading.Tasks;
using Core.Client.Extensions;
using Core.Service.Interfaces.Types;
using Game.Scripts.Extensions;
using Game.Scripts.Extensions.Services;
using Game.Scripts.Transitions;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Scripts.Singletons;

/// <summary>
/// Gerencia o estado do cliente no jogo.
/// Este nó deve ser o último Autoload a ser carregado no projeto.
/// Sua principal responsabilidade é iniciar o processo de carregamento do jogo
/// após todos os Autoloads terem adicionado seus itens de carregamento.
/// Também permite a adição de novos itens de carregamento antes de iniciar o processo.
/// </summary>

public partial class ClientManager : Node
{
    public override void _Ready()
    {
        var loadingScript = GetTree().CurrentScene as LoadingScript;
        // Adicionar itens de carregamento aqui caso necessário...
        // Outros nós podem adicionar itens no carregamento, mas apenas este vai iniciar o carregamento no método abaixo.
        
        
        loadingScript?.StartLoading(openNextScene: true);
    }
    
    public void ExitGame()
    {
        GetTree().Quit();
    }
    
    public override void _ExitTree()
    {
        // Liberar recursos do ClientManager...
    }
}