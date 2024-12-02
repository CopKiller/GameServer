using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;

namespace Core.Client.Network;

public class ClientNetworkService(
    INetworkService networkService, 
    IClientNetworkProcessor packetProcessor,
    IClientConnectionManager connectionManager) : ISingleService
{
    public IServiceConfiguration Configuration { get; } = new ClientNetworkConfiguration();

    public void Start()
    {
        Configuration.Enabled = networkService.Initialize(NetworkMode.Client, address: "127.0.0.1", port: 9050);
        var serverPeer = connectionManager.GetServerPeer();
        packetProcessor.Initialize(serverPeer);
    }

    public void Stop()
    {
        networkService.Stop();
        packetProcessor.Stop();
    }

    public void Update(long currentTick)
    {
        networkService.Update();
    }

    public void Register()
    {
        networkService.Register();
    }

    public void Restart()
    {
        Stop();
        Start();
    }
    
    public void Dispose()
    {
        Stop();
    }
}