using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Network;
using Core.Network.Connection;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Packets.Client;
using Core.Network.Packets.Server;
using Core.Server.Network;
using Core.Server.Network.Interface;
using Core.Service;
using Core.Service.Interfaces.Types;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Server.Network;

public class NetworkIntegrationTests
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

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
                timeout: TimeSpan.FromSeconds(10),
                pollingInterval: TimeSpan.FromMilliseconds(100)
            );

            serverConnectionManager.HasConnectedPeers.Should().BeTrue("Peers should be connected after initialization.");

            serverConnectionManager.DisconnectAll();

            await WaitForConditionAsync(
                () => !serverConnectionManager.HasConnectedPeers,
                timeout: TimeSpan.FromSeconds(5),
                pollingInterval: TimeSpan.FromMilliseconds(100)
            );

            serverConnectionManager.HasConnectedPeers.Should().BeFalse("Peers should be disconnected after calling DisconnectAll.");
            
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

            var clientPacketProcessor = clientManager.ServiceProvider?.GetRequiredService<IClientNetworkProcessor>();
            var clientConnectionManager = clientManager.ServiceProvider?.GetRequiredService<IClientConnectionManager>();
            var serverPacketProcessor = serverManager.ServiceProvider?.GetRequiredService<IServerNetworkProcessor>();
            var serverConnectionManager = serverManager.ServiceProvider?.GetRequiredService<IServerConnectionManager>();

            clientPacketProcessor.Should().NotBeNull();
            clientConnectionManager.Should().NotBeNull();
            serverPacketProcessor.Should().NotBeNull();
            serverConnectionManager.Should().NotBeNull();

            RegisterPacketHandlers(clientPacketProcessor, serverPacketProcessor);

            var serverPacket1 = new CPacketFirst();
            var serverPacket2 = new CPacketSecond();

            Func<Task> serverSendPackets = async () =>
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

    private (ServiceManager serverManager, ServiceManager clientManager) SetupServerAndClient()
    {
        var serverManager = CreateAndStartManager(ConfigureServerNetworkService);
        var clientManager = CreateAndStartManager(ConfigureClientNetworkService);
        return (serverManager, clientManager);
    }

    private ServiceManager CreateAndStartManager(Action<IServiceCollection> configureServices)
    {
        var services = new ServiceCollection();
        ConfigureLoggerService(services);
        ConfigureNetworkService(services);
        configureServices(services);

        var manager = new ServiceManager(services);
        manager.Register();
        manager.Start();
        return manager;
    }

    private void RegisterPacketHandlers(IClientNetworkProcessor clientProcessor, IServerNetworkProcessor serverProcessor)
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
            {
                throw new TimeoutException("A condição não foi atendida dentro do tempo limite.");
            }

            await Task.Delay(pollingInterval);
        }
    }

    private static void ConfigureLoggerService(IServiceCollection services)
    {
        services.AddLogging(); // Simplificado para testes
    }

    private static void ConfigureNetworkService(IServiceCollection services)
    {
        services.AddScoped<ICustomDataWriter, CustomDataWriter>();
        services.AddSingleton<ICustomPacketProcessor, CustomPacketProcessor>();
        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddSingleton<ICustomEventBasedNetListener, CustomEventBasedNetListener>();
    }

    private static void ConfigureServerNetworkService(IServiceCollection services)
    {
        services.AddSingleton<IServerNetworkProcessor, ServerNetworkProcessor>();
        services.AddSingleton<IServerConnectionManager, ServerConnectionManager>();
        services.AddSingleton<ISingleService, ServerNetworkService>();
    }

    private static void ConfigureClientNetworkService(IServiceCollection services)
    {
        services.AddSingleton<IClientNetworkProcessor, ClientNetworkProcessor>();
        services.AddSingleton<IClientConnectionManager, ClientConnectionManager>();
        services.AddSingleton<ISingleService, ClientNetworkService>();
    }
}
