using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network;

public class ClientNetworkService(
    INetworkManager networkManager,
    IClientPacketProcessor packetProcessor,
    IClientConnectionManager connectionManager) : ISingleService
{
    public IServiceConfiguration ServiceConfiguration { get; } = new ClientNetworConfiguration();

    // ISingleService
    public void Start()
    {
        networkManager.StartClient();

        packetProcessor.Initialize();

        ServiceConfiguration.Enabled = true;
    }

    public void Stop()
    {
        networkManager.Stop();
        packetProcessor.Stop();
        connectionManager.DisconnectFromServer();
        ServiceConfiguration.Enabled = false;
    }

    public void Update(long currentTick)
    {
        networkManager.PollEvents();
    }

    public void Register() { }

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