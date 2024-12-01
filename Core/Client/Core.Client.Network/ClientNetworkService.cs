using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;

namespace Core.Client.Network;

public class ClientNetworkService(
    INetworkService networkService, 
    ICustomPacketProcessor packetProcessor, 
    ICustomEventBasedNetListener netListener) : ISingleService, IClientNetworkService
{
    
    private readonly ClientNetworkProcessor _processor = new(packetProcessor, netListener);
    
    public IServiceConfiguration Configuration { get; } = new ClientNetworkConfiguration();
    
    public bool IsConnected => networkService.IsRunning;
    
    public ICustomNetPeer? GetServerPeer()
    {
        return networkService.GetFirstPeer();
    }

    public void Start()
    {
        _processor.Initialize();
        networkService.Initialize(NetworkMode.Client, address: "127.0.0.1", port: 9050);
    }

    public void Stop()
    {
        networkService.Stop();
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