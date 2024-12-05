using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Server.Network.Interface;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network;

public class ServerNetworkService(
    INetworkConfiguration networkConfiguration,
    INetworkManager networkManager,
    IServerPacketProcessor packetProcessor) : ISingleService
{
    public IServiceConfiguration ServiceConfiguration { get; } = new ServiceConfiguration();

    public void Start()
    {
        packetProcessor.Initialize();

        ServiceConfiguration.Enabled = networkManager.StartListener(networkConfiguration.Port);
    }

    public void Stop()
    {
        networkManager.Stop();
    }

    public void Update(long currentTick)
    {
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