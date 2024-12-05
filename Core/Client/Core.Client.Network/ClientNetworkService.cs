using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network;

public class ClientNetworkService(
    INetworkConfiguration networkConfiguration,
    INetworkManager networkManager,
    IClientPacketProcessor packetProcessor,
    ILogger<ClientNetworkService> logger) : ISingleService
{
    public IServiceConfiguration ServiceConfiguration { get; } = new ServiceConfiguration();

    // ISingleService
    public void Start()
    {
        if (ServerPeer is { IsConnected: true })
        {
            logger.LogError("Server peer is already connected.");
            return;
        }

        networkManager.StartClient();
        ServerPeer = networkManager.ConnectToServer(networkConfiguration.Address, networkConfiguration.Port,
            networkConfiguration.Key);

        if (ServerPeer is null)
        {
            logger.LogError("Failed to connect to the server : ServerPeer is null.");
            return;
        }

        packetProcessor.Initialize(ServerPeer);

        ServiceConfiguration.Enabled = true;
    }

    public void Stop()
    {
        networkManager.Stop();
        packetProcessor.Stop();
        ServiceConfiguration.Enabled = false;
    }

    public void Update(long currentTick)
    {
        if (ServerPeer is null)
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

    // IClientNetworkService

    public ICustomNetPeer? ServerPeer { get; private set; }
}