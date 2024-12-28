using System.Threading.Tasks;
using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Service.Interfaces.Types;
using Game.Scripts.Transitions;
using Godot;

namespace Game.Scripts.Singletons;

public partial class NetworkManager : Node
{
    private ISingleService _clientNetworkManager;
    private IClientPacketProcessor _clientPacketProcessor;
    
    public override void _Ready()
    {
        var loadingScript = GetTree().CurrentScene as LoadingScript;

        loadingScript?.AddTask(ConfigureNetwork, "Configurando rede...");
        loadingScript?.AddTask(StartNetwork, "Iniciando rede...");
    }
    
    private async Task ConfigureNetwork()
    {
        await Task.Run(() =>
        {
            _clientNetworkManager = ServiceManager.GetRequiredService<ISingleService>();
            _clientPacketProcessor = ServiceManager.GetRequiredService<IClientPacketProcessor>();
        });

        await Task.Delay(500);
    }
    
    private async Task StartNetwork()
    {
        await Task.Run(() =>
        {
            _clientNetworkManager.Start();
        });

        var connectionManager = ServiceManager.GetRequiredService<IClientConnectionManager>();
        
        while (connectionManager.IsConnected != true)
        {
            await Task.Delay(1000);
            _clientNetworkManager.Start();
        }
    }
}