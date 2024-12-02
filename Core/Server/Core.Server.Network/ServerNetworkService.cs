using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Server.Network.Interface;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network;

public class ServerNetworkService : ISingleService
{
    private readonly INetworkService networkService;
    private readonly IServerConnectionManager connectionManager;
    private readonly IServerNetworkProcessor networkProcessor;
    private readonly ILogger<ServerNetworkProcessor> logger;
    public ServerNetworkService(
        INetworkService networkService,
        IServerConnectionManager connectionManager,
        IServerNetworkProcessor networkProcessor,
        ILogger<ServerNetworkProcessor> logger)
    {
        this.networkService = networkService;
        this.connectionManager = connectionManager;
        this.networkProcessor = networkProcessor;
        this.logger = logger;
    }
    
    public readonly NetworkMode NetworkMode = NetworkMode.Server;
    public readonly int Port = 9050;
    
    public IServiceConfiguration Configuration { get; } = new ServerNetworkConfiguration();

    public void Start()
    {
        networkProcessor.Initialize();
        Configuration.Enabled = networkService.Initialize(NetworkMode, port: Port);
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