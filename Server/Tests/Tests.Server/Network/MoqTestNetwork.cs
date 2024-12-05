using System.Threading.Channels;
using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Network;
using Core.Network.Connection;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Packets.Server;
using Core.Server.Network;
using Core.Server.Network.Interface;
using Core.Service;
using Core.Service.Interfaces.Types;
using FluentAssertions;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Server.Network;

public class NetworkTests
{
    #region Client Tests

    [Fact]
    public void Client_SendPacket_ShouldCallSendPacketMethod()
    {
        // Arrange
        var mockProcessor = new Mock<IClientPacketProcessor>();
        var mockPeer = new Mock<ICustomNetPeer>();

        var packet = new SPacketFirst();

        // Act
        mockProcessor.Object.SendPacket(packet);

        // Assert
        mockProcessor.Verify(p => p.SendPacket(packet, CustomDeliveryMethod.ReliableOrdered), Times.Once);
    }

    [Fact]
    public void Client_RegisterPacket_ShouldInvokeCallbackOnReceive()
    {
        // Arrange
        var mockProcessor = new Mock<IClientPacketProcessor>();
        var mockPeer = new Mock<ICustomNetPeer>();
        var callbackInvoked = false;

        mockProcessor.Setup(p => p.RegisterPacket<SPacketFirst>(It.IsAny<Action<SPacketFirst, ICustomNetPeer>>()))
            .Callback((Action<SPacketFirst, ICustomNetPeer> action) =>
            {
                callbackInvoked = true;
                action(new SPacketFirst(), mockPeer.Object);
            });

        // Act
        mockProcessor.Object.RegisterPacket<SPacketFirst>((packet, peer) => { });

        // Assert
        Assert.True(callbackInvoked);
    }

    #endregion

    #region Server Tests

    [Fact]
    public void Server_SendPacket_ShouldCallSendPacketMethod()
    {
        // Arrange
        var mockProcessor = new Mock<IServerPacketProcessor>();
        var mockPeer = new Mock<ICustomNetPeer>();

        var packet = new SPacketFirst();

        // Act
        mockProcessor.Object.SendPacket(mockPeer.Object, packet);

        // Assert
        mockProcessor.Verify(p => p.SendPacket(mockPeer.Object, packet, CustomDeliveryMethod.ReliableOrdered),
            Times.Once);
    }

    [Fact]
    public void Server_HasConnectedPeers_ShouldReturnTrueIfPeersExist()
    {
        // Arrange
        var mockConnectionManager = new Mock<IServerConnectionManager>();
        mockConnectionManager.Setup(cm => cm.HasConnectedPeers).Returns(true);

        // Act
        var result = mockConnectionManager.Object.HasConnectedPeers;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Server_DisconnectPeer_ShouldCallDisconnectMethod()
    {
        // Arrange
        var mockConnectionManager = new Mock<IServerConnectionManager>();
        var mockPeer = new Mock<ICustomNetPeer>();

        // Act
        mockConnectionManager.Object.DisconnectPeer(mockPeer.Object);

        // Assert
        mockConnectionManager.Verify(cm => cm.DisconnectPeer(mockPeer.Object, "Disconnected"), Times.Once);
    }

    [Fact]
    public void Server_GetPeerById_ShouldReturnPeer()
    {
        // Arrange
        var mockPeer = new Mock<ICustomNetPeer>();
        var mockConnectionManager = new Mock<IServerConnectionManager>();

        mockConnectionManager.Setup(cm => cm.GetPeerById(It.IsAny<int>())).Returns(mockPeer.Object);

        // Act
        var peer = mockConnectionManager.Object.GetPeerById(1);

        // Assert
        Assert.NotNull(peer);
        Assert.Equal(mockPeer.Object, peer);
    }

    #endregion
}