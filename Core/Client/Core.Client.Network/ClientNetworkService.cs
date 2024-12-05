using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network;

public class ClientNetworkService(
    INetworkConfiguration networkConfiguration,
    INetworkManager networkManager,
    IClientPacketProcessor packetProcessor,
    IClientConnectionManager connectionManager,
    ILogger<ClientNetworkService> logger) : ISingleService
{
    public IServiceConfiguration ServiceConfiguration { get; } = new ServiceConfiguration();

    // ISingleService
    public void Start()
    {
        var serverPeer = connectionManager.GetServerPeer();

        if (serverPeer is not null)
        {
            logger.LogError("Server peer is already connected.");
            return;
        }

        networkManager.StartClient();
        
        var peer = networkManager.ConnectToServer(networkConfiguration.Address, networkConfiguration.Port,
            networkConfiguration.Key);
        
        connectionManager.SetServerPeer(peer);

        packetProcessor.Initialize(peer);

        ServiceConfiguration.Enabled = true;
    }

    public void Stop()
    {
        networkManager.Stop();
        packetProcessor.Stop();
        connectionManager.Disconnect();
        ServiceConfiguration.Enabled = false;
    }

    public void Update(long currentTick)
    {
        if (connectionManager.GetServerPeer() is null)
        {
            logger.LogError("Update service failed: Server peer is null.");
            Stop();
            return;
        }

        networkManager.PollEvents();
    }

    public void Register()
    {
        networkConfiguration.AutoRecycle = true;
        networkConfiguration.EnableStatistics = false;
        networkConfiguration.UnconnectedMessagesEnabled = false;
        networkConfiguration.UseNativeSockets = true;
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