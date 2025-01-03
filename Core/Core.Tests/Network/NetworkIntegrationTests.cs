
using Core.Client.Extensions;
using Core.Client.Network.Interface;
using Core.Logger.Interface;
using Core.Network.Packets.Client;
using Core.Network.Packets.Server;
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

public class NetworkIntegrationTests
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

            var clientPacketProcessor = clientManager.ServiceProvider?.GetRequiredService<IClientPacketProcessor>();
            var clientConnectionManager = clientManager.ServiceProvider?.GetRequiredService<IClientConnectionManager>();
            var serverPacketProcessor = serverManager.ServiceProvider?.GetRequiredService<IServerPacketProcessor>();
            var serverConnectionManager = serverManager.ServiceProvider?.GetRequiredService<IServerConnectionManager>();

            clientPacketProcessor.Should().NotBeNull();
            clientConnectionManager.Should().NotBeNull();
            serverPacketProcessor.Should().NotBeNull();
            serverConnectionManager.Should().NotBeNull();

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
                Position = new PositionDto(),
                Vitals = new VitalsDto(),
                Stats = new StatsDto()
            };

            var serverSendPackets = async () =>
            {
                serverPacketProcessor.SendPacketToAll(serverPacket1);
                serverPacketProcessor.SendPacketToAll(serverPacket2);
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
        ClientServiceExtensions.AddNetworkClient(clientCollection);
        var clientManager = CreateAndStartManager(clientCollection);
        
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

    private void RegisterPacketHandlers(IClientPacketProcessor iClientProcessor, IServerPacketProcessor serverProcessor)
    {
        iClientProcessor.RegisterPacket<CPacketFirst>((packet, peer) =>
        {
            packet.Should().NotBeNull();
            peer.Should().NotBeNull();
        });

        iClientProcessor.RegisterPacket<CPacketSecond>((packet, peer) =>
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