
using Core.Client.Extensions;
using Core.Client.Network.Interface;
using Core.Logger.Interface;
using Core.Network.Interface.Packet;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Player;
using Core.Server.Extensions;
using Core.Server.Network.Interface;
using Core.Service.Interfaces.Types;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Core.Tests.Network;

public partial class NetworkTests
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    [Fact]
    public async Task Server_ShouldInitializeAndAcceptConnections()
    {
        await ExecuteTestAsync(async () =>
        {
            var (serverManager, clientManager) = SetupServerAndClient();

            var serverConnectionManager = serverManager.ServiceProvider?.GetRequiredService<IServerConnectionManager>();
            serverConnectionManager.Should().NotBeNull();

            await WaitForConditionAsync(
                () => serverConnectionManager.HasConnectedPeers,
                TimeSpan.FromSeconds(10),
                TimeSpan.FromMilliseconds(100)
            );

            serverConnectionManager.HasConnectedPeers.Should()
                .BeTrue("Peers should be connected after initialization.");

            serverConnectionManager.DisconnectAll();

            await WaitForConditionAsync(
                () => !serverConnectionManager.HasConnectedPeers,
                TimeSpan.FromSeconds(5),
                TimeSpan.FromMilliseconds(100)
            );

            serverConnectionManager.HasConnectedPeers.Should()
                .BeFalse("Peers should be disconnected after calling DisconnectAll.");

            serverManager.Stop();
            clientManager.Stop();

            serverManager.Dispose();
            clientManager.Dispose();
        });
    }

    [Fact]
    public async Task Client_ShouldSendPacketsSuccessfully()
    {
        await ExecuteTestAsync(async () =>
        {
            var (serverManager, clientManager) = SetupServerAndClient();

            var clientPacketProcessor = clientManager.ServiceProvider?.GetRequiredService<IPacketRegister>();
            var clientConnectionManager = clientManager.ServiceProvider?.GetRequiredService<IClientConnectionManager>();
            var serverPacketProcessor = serverManager.ServiceProvider?.GetRequiredService<IPacketRegister>();
            var serverConnectionManager = serverManager.ServiceProvider?.GetRequiredService<IServerConnectionManager>();
            
            var serverPackerSender = serverManager.ServiceProvider?.GetRequiredService<IPacketSender>();
            
            clientPacketProcessor.Should().NotBeNull();
            clientConnectionManager.Should().NotBeNull();
            serverPacketProcessor.Should().NotBeNull();
            serverConnectionManager.Should().NotBeNull();
            serverPackerSender.Should().NotBeNull();

            RegisterPacketHandlers(clientPacketProcessor, serverPacketProcessor);

            var serverPacket1 = new CPacketFirst();
            serverPacket1.Name = "Test";
            serverPacket1.Age = 20;
            
            var serverPacket2 = new CPacketSecond();
            serverPacket2.Player = new PlayerDto
            {
                Name = "Test",
                Level = 20,
                Golds = 10,
                Diamonds = 15,
                Position = new Vector2(),
                Vitals = new VitalsDto(),
                Stats = new StatsDto()
            };

            var serverSendPackets = async () =>
            {
                serverPackerSender.SendPacketToAll(serverPacket1);
                serverPackerSender.SendPacketToAll(serverPacket2);
                await Task.Delay(1000);
            };

            await serverSendPackets.Should().NotThrowAsync();

            serverManager.Stop();
            clientManager.Stop();

            serverManager.Dispose();
            clientManager.Dispose();
        });
    }

    private async Task ExecuteTestAsync(Func<Task> testLogic)
    {
        await _semaphore.WaitAsync();

        try
        {
            await testLogic();
        }
        finally
        {
            await Task.Delay(3000); // Simula um pequeno delay para garantir finalização
            _semaphore.Release();
        }
    }

    private (IServiceManager serverManager, IServiceManager clientManager) SetupServerAndClient()
    {
        var serverCollection = new ServiceCollection();
        serverCollection.AddNetworkServer();
        var serverManager = CreateAndStartManager(serverCollection);
        
        var clientCollection = new ServiceCollection();
        clientCollection.AddNetworkClient();
        var clientManager = CreateAndStartManager(clientCollection);
        
        var clientConnectionManager = clientManager.ServiceProvider?.GetRequiredService<IClientConnectionManager>();
        
        clientConnectionManager.Should().NotBeNull();
        
        clientConnectionManager.ConnectToServer();
        
        return (serverManager, clientManager);
    }

    private IServiceManager CreateAndStartManager(IServiceCollection services)
    {
        ServerServiceExtensions.AddCryptography(services);
        ServerServiceExtensions.AddLogger(services, LogLevel.Debug);
        ServerServiceExtensions.AddNetwork(services);
        services.AddMapper();
        ServerServiceExtensions.AddServiceManager(services);
        services.AddSingleton<ILogOutput, LoggerOutput>();

        var manager = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = false,
                ValidateScopes = false
            })
            .GetRequiredService<IServiceManager>();
        manager.Register();
        manager.Start();
        return manager;
    }

    private void RegisterPacketHandlers(IPacketRegister clientProcessor, IPacketRegister serverProcessor)
    {
        clientProcessor.RegisterPacket<CPacketFirst>((packet, peer) =>
        {
            packet.Should().NotBeNull();
            peer.Should().NotBeNull();
        });

        clientProcessor.RegisterPacket<CPacketSecond>((packet, peer) =>
        {
            packet.Should().NotBeNull();
            peer.Should().NotBeNull();
            
            packet.Player.Should().NotBeNull();
            packet.Player.Name.Should().NotBeNullOrEmpty();
            packet.Player.Level.Should().BeGreaterThan(0);
            packet.Player.Golds.Should().BeGreaterThan(0);
            packet.Player.Diamonds.Should().BeGreaterThan(0);
        });

        serverProcessor.RegisterPacket<SPacketFirst>((packet, peer) =>
        {
            packet.Should().NotBeNull();
            peer.Should().NotBeNull();
        });

        serverProcessor.RegisterPacket<SPacketSecond>((packet, peer) =>
        {
            packet.Should().NotBeNull();
            peer.Should().NotBeNull();
        });
    }

    private async Task WaitForConditionAsync(Func<bool> condition, TimeSpan timeout, TimeSpan pollingInterval)
    {
        var startTime = DateTime.UtcNow;

        while (!condition())
        {
            if (DateTime.UtcNow - startTime > timeout)
                throw new TimeoutException("A condição não foi atendida dentro do tempo limite.");

            await Task.Delay(pollingInterval);
        }
    }
}

public class CPacketFirst
{
    public string Name { get; set; }
    
    public int Age { get; set; }
}

public class CPacketSecond
{
    public int Id { get; set; }
    public PlayerDto Player { get; set; }
}