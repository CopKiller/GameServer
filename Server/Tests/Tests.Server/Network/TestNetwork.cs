using System.Threading.Channels;
using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Network;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Server.Network;
using Core.Server.Network.Interface;
using Core.Service;
using Core.Service.Interfaces.Types;
using FluentAssertions;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.Server.Network;


public class NetworkTests
{
    private IServiceCollection _clientServices;
    private IServiceCollection _serverServices;
    
    private ServiceManager _clientManager;
    private ServiceManager _serverManager;
    
    public NetworkTests()
    {
        _clientServices = new ServiceCollection();
        _serverServices = new ServiceCollection();
        
        _clientManager = new ServiceManager(_clientServices);
        _serverManager = new ServiceManager(_serverServices);
        
        ConfigureClientServices(_clientServices);
        ConfigureServerServices(_serverServices);
        
        _clientManager.Register();
        _serverManager.Register();
    }

    private void ConfigureClientServices(IServiceCollection services)
    {
        const LogLevel logLevel = LogLevel.Trace;

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(logLevel);
            loggingBuilder.AddProvider(new CustomLoggerProvider(logLevel));
        });

        services.AddSingleton<ICustomPacketProcessor, CustomPacketProcessor>();
        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<ICustomEventBasedNetListener, CustomEventBasedNetListener>();
        
        services.AddSingleton<ClientNetworkService>();
        services.AddSingleton<ISingleService>(provider => provider.GetRequiredService<ClientNetworkService>());
        services.AddSingleton<IClientNetworkService>(provider => provider.GetRequiredService<ClientNetworkService>());
        
        _clientManager = new ServiceManager(_clientServices);
    }

    private void ConfigureServerServices(IServiceCollection services)
    {
        const LogLevel logLevel = LogLevel.Trace;

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(logLevel);
            loggingBuilder.AddProvider(new CustomLoggerProvider(logLevel));
        });

        services.AddSingleton<ICustomPacketProcessor, CustomPacketProcessor>();
        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<ICustomEventBasedNetListener, CustomEventBasedNetListener>();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        
        services.AddSingleton<ServerNetworkService>();
        services.AddSingleton<ISingleService>(provider => provider.GetRequiredService<ServerNetworkService>());
        services.AddSingleton<IServerNetworkService>(provider => provider.GetRequiredService<ServerNetworkService>());
        
        _serverManager = new ServiceManager(_serverServices);
    }

    [Fact]
    public async Task ClientCanConnectToServer()
    {
        // Arrange
        var clientProvider = _clientManager.ServiceProvider;
        var serverProvider = _serverManager.ServiceProvider;
        
        var clientNetworkService = clientProvider.GetRequiredService<ISingleService>();
        var serverNetworkService = serverProvider.GetRequiredService<ISingleService>();
        
        var serverListener = serverProvider.GetRequiredService<ICustomEventBasedNetListener>();
        var connectionManager = serverProvider.GetRequiredService<IConnectionManager>();
        //serverListener.OnConnectionRequest += request => request.Accept();
        
        //_serverManager.Register();
        //_serverManager.Start();
        
        serverNetworkService.Register();
        serverNetworkService.Start();
        
        /*clientProvider.GetRequiredService<INetworkService>().Register();
        clientProvider.GetRequiredService<INetworkService>().Initialize();*/
        
        await Task.Delay(1000); // Aguarda o servidor inicializar
        
        clientNetworkService.Register();
        clientNetworkService.Start();
        
        //_clientManager.Register();
        //_clientManager.Start();
        
        await Task.Delay(1000); // Aguarda o cliente conectar ao servidor

        
        if (clientNetworkService is ClientNetworkService clientNetwork)
        {
            clientNetwork.IsConnected.Should().BeTrue();
        }
        
        clientNetworkService.Update(1);
        serverNetworkService.Update(1);
        
        if (serverNetworkService is ServerNetworkService serverNetwork)
        {
            serverNetwork.GetConnectedPlayersCount().Should().BeGreaterThanOrEqualTo(1);
        }
    }

    //[Fact]
    public async Task ClientAndServerExchangePacketsSuccessfully()
    {
        // Arrange
        var clientProvider = _clientManager.ServiceProvider;
        var serverProvider = _serverManager.ServiceProvider;

        var clientNetworkService = clientProvider.GetRequiredService<IClientNetworkService>();
        var serverNetworkService = serverProvider.GetRequiredService<IServerNetworkService>();

        var serverListener = serverProvider.GetRequiredService<ICustomEventBasedNetListener>();
        var tcs = new TaskCompletionSource<Packet1>();

        serverListener.OnConnectionRequest += request => request.Accept();
        serverListener.OnNetworkReceive += (fromPeer, dataReader, channel, deliveryMethod) =>
        {
            var receivedPacket = dataReader.Get(() => new Packet1());
            tcs.SetResult(receivedPacket);
        };

        // Act
        var serverTask = Task.Run(() =>
        {
            _serverManager.Start();
        });
        await Task.Delay(100); // Aguarda o servidor inicializar

        var clientTask = Task.Run(() =>
        {
            _clientManager.Start();
        });
        await Task.WhenAll(serverTask, clientTask);
        
        var packetProcessor = clientProvider.GetRequiredService<ICustomPacketProcessor>();

        var packetToSend = new Packet1 { Message = "Hello, Server!" };

        var serverPeer = clientNetworkService.GetServerPeer();
        
        serverPeer.Should().NotBeNull();
        
        packetProcessor.SendPacket(serverPeer, packetToSend, CustomDeliveryMethod.ReliableOrdered);
        var receivedPacket = await tcs.Task; // Aguarda até que o pacote seja recebido

        // Assert
        receivedPacket.Should().NotBeNull();
        receivedPacket.Message.Should().Be("Hello, Server!");
    }

    public class Packet1 : ICustomSerializable
    {
        public string Message { get; set; }

        public void Deserialize(ICustomDataReader reader)
        {
            Message = reader.GetString();
        }

        public void Serialize(ICustomDataWriter writer)
        {
            writer.Put(Message);
        }
    }
}