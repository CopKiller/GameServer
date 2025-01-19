
using Core.Database.Models.Player;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Packet;
using Core.Network.SerializationObjects;
using Core.Server.Network.Interface;
using Moq;
using Xunit;

namespace Core.Tests.Network;

public partial class NetworkTests
{
    #region Client Tests

    [Fact]
    public void Client_SendPacket_ShouldCallSendPacketMethod()
    {
        // Arrange
        var mockProcessor = new Mock<IPacketSender>();
        var mockPeer = new Mock<IAdapterNetPeer>();

        var packet = new SPacketFirst();

        // Act
        mockProcessor.Object.SendPacket(mockPeer.Object, packet);

        // Assert
        mockProcessor.Verify(p => p.SendPacket(mockPeer.Object, packet, CustomDeliveryMethod.ReliableOrdered), Times.Once);
    }

    [Fact]
    public void Client_RegisterPacket_ShouldInvokeCallbackOnReceive()
    {
        // Arrange
        var mockProcessor = new Mock<IPacketRegister>();
        var mockPeer = new Mock<IAdapterNetPeer>();
        var callbackInvoked = false;

        mockProcessor.Setup(p => p.RegisterPacket<SPacketFirst>(It.IsAny<Action<SPacketFirst, IAdapterNetPeer>>()))
            .Callback((Action<SPacketFirst, IAdapterNetPeer> action) =>
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
        var mockProcessor = new Mock<IPacketSender>();
        var mockPeer = new Mock<IAdapterNetPeer>();

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
        var mockPeer = new Mock<IAdapterNetPeer>();

        // Act
        mockConnectionManager.Object.DisconnectPeer(mockPeer.Object);

        // Assert
        mockConnectionManager.Verify(cm => cm.DisconnectPeer(mockPeer.Object, "Disconnected"), Times.Once);
    }

    [Fact]
    public void Server_GetPeerById_ShouldReturnPeer()
    {
        // Arrange
        var mockPeer = new Mock<IAdapterNetPeer>();
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

public class SPacketFirst
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public SPacketFirst()
    {
        Id = 1;
        Name = "First Packet";
    }
}

public class SPacketSecond
{
    public int Id { get; set; }
    public PlayerDto Player { get; set; }
    
    public SPacketSecond()
    {
        Player = new PlayerDto();
        Player.Id = 1;
    }
}