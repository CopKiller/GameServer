using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Server.Network.Interface;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network;

public class ServerNetworkService(
    IServerConnectionManager connectionManager,
    INetService netService) : ISingleService
{
    public IServiceConfiguration ServiceConfiguration { get; } = new ServiceConfiguration();

    public void Start()
    {
        ServiceConfiguration.Enabled = connectionManager.StartListener();
    }

    public void Stop()
    {
        netService.Stop();
        connectionManager.DisconnectAll();
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