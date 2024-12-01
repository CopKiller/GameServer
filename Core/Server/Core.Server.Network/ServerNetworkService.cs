using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Server.Network.Interface;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network;

public class ServerNetworkService(INetworkService networkService, 
    ICustomPacketProcessor packetProcessor, ICustomEventBasedNetListener netListener,
    IConnectionManager connectionManager,
    ILogger<ServerNetworkProcessor> logger) : ISingleService, IServerNetworkService
{
    private readonly ServerNetworkProcessor _processor = new(packetProcessor, netListener, connectionManager, logger);
    
    public IServiceConfiguration Configuration { get; } = new ServerNetworkConfiguration();

    public void Start()
    {
        _processor.Initialize();
        networkService.Initialize(NetworkMode.Server, port: 9050);
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


    public int GetConnectedPlayersCount()
    {
        return connectionManager.GetAllPeers().Count();
    }

    public void SendPacketToAllPlayers<T>(T packet) where T : ICustomSerializable
    {
        // TODO: Implement
        //throw new NotImplementedException();
    }
}