using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network;

public class ClientNetworkService(
    INetService netService,
    IClientConnectionManager connectionManager) : ISingleService
{
    public IServiceConfiguration ServiceConfiguration { get; } = new ClientNetworkConfiguration();

    // ISingleService
    public void Start()
    {
        netService.Start();

        ServiceConfiguration.Enabled = true;
    }

    public void Stop()
    {
        netService.Stop();
        connectionManager.DisconnectFromServer();
        ServiceConfiguration.Enabled = false;
    }

    public void Update(long currentTick)
    {
        netService.PollEvents();
    }

    public void Register()
    {
        connectionManager.ConfigureNetworkSettings();
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