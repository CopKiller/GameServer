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
    [Fact]
    public async Task Client_ShouldSendPacketsSuccessfully()
    {
        // Arrange
        var serverService = new ServiceCollection();
        ConfigureLoggerService(serverService);
        ConfigureNetworkService(serverService);
        ConfigureServerNetworkService(serverService);
        
        var serverManager = new ServiceManager(serverService);
        serverManager.Register();
        serverManager.Start();
        
        await Task.Delay(3000); // Simula um pequeno delay para garantir inicialização
        

        var clientService = new ServiceCollection();
        ConfigureLoggerService(clientService);
        ConfigureNetworkService(clientService);
        ConfigureClientNetworkService(clientService);
        
        var clientManager = new ServiceManager(clientService);
        clientManager.Register();
        clientManager.Start();
        
        await Task.Delay(3000); // Simula um pequeno delay para garantir inicialização

        var clientPacketProcessor = clientManager.ServiceProvider?.GetRequiredService<IClientNetworkProcessor>();
        clientPacketProcessor.Should().NotBeNull();
        
        var clientConnectionManager = clientManager.ServiceProvider?.GetRequiredService<IClientConnectionManager>();
        clientConnectionManager.Should().NotBeNull();
        
        var serverPacketProcessor = serverManager.ServiceProvider?.GetRequiredService<IServerNetworkProcessor>();
        serverPacketProcessor.Should().NotBeNull();
        
        var serverConnectionManager = serverManager.ServiceProvider?.GetRequiredService<IServerConnectionManager>();
        serverConnectionManager.Should().NotBeNull();

        // Act
        
        clientPacketProcessor.RegisterPacket<CPacketFirst>((packet, peer) =>
        {
            packet.Should().NotBeNull();
            peer.Should().NotBeNull();
        });
        
        clientPacketProcessor.RegisterPacket<CPacketSecond>((packet, peer) =>
        {
            packet.Should().NotBeNull();
            peer.Should().NotBeNull();
        });

        // Act
        
        serverPacketProcessor.RegisterPacket<SPacketFirst>((packet, peer) =>
        {
            packet.Should().NotBeNull();
            peer.Should().NotBeNull();
        });
        
        serverPacketProcessor.RegisterPacket<SPacketSecond>((packet, peer) =>
        {
            packet.Should().NotBeNull();
            peer.Should().NotBeNull();
        });
        
        var serverpacket1 = new CPacketFirst();
        var serverpacket2 = new CPacketSecond();

        Func<Task> serverSendPackets = async () =>
        {
            serverPacketProcessor.SendPacketToAll(serverpacket1);
            serverPacketProcessor.SendPacketToAll(serverpacket2);
            await Task.Delay(1000); // Simula um pequeno delay para garantir envio
        };

        // Assert
        await serverSendPackets.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Server_ShouldInitializeAndAcceptConnections()
    {
        // Arrange
        var serverService = new ServiceCollection();
        ConfigureLoggerService(serverService);
        ConfigureNetworkService(serverService);
        ConfigureServerNetworkService(serverService);
        
        var serverManager = new ServiceManager(serverService);
        serverManager.Register();
        serverManager.Start();
        
        await Task.Delay(3000); // Simula um pequeno delay para garantir inicialização
        

        var clientService = new ServiceCollection();
        ConfigureLoggerService(clientService);
        ConfigureNetworkService(clientService);
        ConfigureClientNetworkService(clientService);
        
        var clientManager = new ServiceManager(clientService);
        clientManager.Register();
        clientManager.Start();
        
        await Task.Delay(4000); // Simula um pequeno delay para garantir inicialização
        
        var serverConnectionManager = serverManager.ServiceProvider?.GetRequiredService<IServerConnectionManager>();
        serverConnectionManager.Should().NotBeNull();
        
        // Act
        var hasPeers = serverConnectionManager.HasConnectedPeers;
        
        // Assert
        hasPeers.Should().BeTrue("Peers should be connected after initialization.");
        
        // Act
        serverConnectionManager.DisconnectAll();
        
        await Task.Delay(1000); // Simula um pequeno delay para garantir desconexão
        
        // Assert
        hasPeers = serverConnectionManager.HasConnectedPeers;
        hasPeers.Should().BeFalse("Peers should be disconnected after calling DisconnectAll.");
        
    }

    private static void ConfigureLoggerService(IServiceCollection services)
    {
        services.AddLogging(); // Simplificado para testes
    }

    private static void ConfigureNetworkService(IServiceCollection services)
    {
        // Network configuration for both Client and Server
        services.AddScoped<ICustomDataWriter, CustomDataWriter>();
        services.AddSingleton<ICustomPacketProcessor, CustomPacketProcessor>();
        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddSingleton<ICustomEventBasedNetListener, CustomEventBasedNetListener>();
    }
    
    private static void ConfigureServerNetworkService(IServiceCollection services)
    {
        // Server-specific
        services.AddSingleton<IServerNetworkProcessor, ServerNetworkProcessor>();
        services.AddSingleton<IServerConnectionManager, ServerConnectionManager>();
        services.AddSingleton<ISingleService, ServerNetworkService>();
    }
    
    private static void ConfigureClientNetworkService(IServiceCollection services)
    {
        // Client-specific
        services.AddSingleton<IClientNetworkProcessor, ClientNetworkProcessor>();
        services.AddSingleton<IClientConnectionManager, ClientConnectionManager>();
        services.AddSingleton<ISingleService, ClientNetworkService>();
    }
}
