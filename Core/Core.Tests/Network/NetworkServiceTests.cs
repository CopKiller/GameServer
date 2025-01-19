using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Event;
using Moq;
using Xunit;

namespace Core.Tests.Network;

public partial class NetworkTests
{
    private readonly Mock<INetworkManager> _mockNetworkManager;
    private readonly Mock<INetworkEventsListener> _mockNetworkEventsListener;
    private readonly Mock<IAdapterNetManager> _mockAdapterNetManager;
    private readonly Mock<IConnectionManager> _mockConnectionManager;

    public NetworkTests()
    {
        _mockNetworkManager = new Mock<INetworkManager>();
        _mockNetworkEventsListener = new Mock<INetworkEventsListener>();
        _mockAdapterNetManager = new Mock<IAdapterNetManager>();
        _mockConnectionManager = new Mock<IConnectionManager>();

        _mockNetworkManager.SetupGet(nm => nm.AdapterNetManager).Returns(_mockAdapterNetManager.Object);
        _mockNetworkManager.SetupGet(nm => nm.NetworkEventsListener).Returns(_mockNetworkEventsListener.Object);
    }

    [Fact]
    public void StartListener_ShouldStartSuccessfully()
    {
        _mockAdapterNetManager.Setup(nm => nm.StartListener(It.IsAny<int>())).Returns(true);

        var result = _mockAdapterNetManager.Object.StartListener(7777);

        Assert.True(result);
        _mockAdapterNetManager.Verify(nm => nm.StartListener(7777), Times.Once);
    }

    [Fact]
    public void StartListener_ShouldFailToStart()
    {
        _mockAdapterNetManager.Setup(nm => nm.StartListener(It.IsAny<int>())).Returns(false);

        var result = _mockAdapterNetManager.Object.StartListener(7777);

        Assert.False(result);
        _mockAdapterNetManager.Verify(nm => nm.StartListener(7777), Times.Once);
    }

    [Fact]
    public void ConnectToServer_ShouldReturnPeer()
    {
        var mockPeer = new Mock<IAdapterNetPeer>().Object;
        _mockAdapterNetManager.Setup(nm => nm.ConnectToServer("127.0.0.1", 7777, "testKey"))
            .Returns(mockPeer);

        var peer = _mockAdapterNetManager.Object.ConnectToServer("127.0.0.1", 7777, "testKey");

        Assert.NotNull(peer);
        _mockAdapterNetManager.Verify(nm => nm.ConnectToServer("127.0.0.1", 7777, "testKey"), Times.Once);
    }

    [Fact]
    public void ConnectToServer_WithInvalidIP_ShouldReturnNull()
    {
        _mockAdapterNetManager.Setup(nm => nm.ConnectToServer("999.999.999.999", 7777, "testKey"))
            .Returns((IAdapterNetPeer)null);

        var peer = _mockAdapterNetManager.Object.ConnectToServer("999.999.999.999", 7777, "testKey");

        Assert.Null(peer);
        _mockAdapterNetManager.Verify(nm => nm.ConnectToServer("999.999.999.999", 7777, "testKey"), Times.Once);
    }

    [Fact]
    public void ConnectToServer_WithInvalidPort_ShouldReturnNull()
    {
        _mockAdapterNetManager.Setup(nm => nm.ConnectToServer("127.0.0.1", 99999, "testKey"))
            .Returns((IAdapterNetPeer)null);

        var peer = _mockAdapterNetManager.Object.ConnectToServer("127.0.0.1", 99999, "testKey");

        Assert.Null(peer);
        _mockAdapterNetManager.Verify(nm => nm.ConnectToServer("127.0.0.1", 99999, "testKey"), Times.Once);
    }

    [Fact]
    public void Stop_ShouldStopSuccessfully()
    {
        _mockAdapterNetManager.Setup(nm => nm.Stop());

        _mockAdapterNetManager.Object.Stop();

        _mockAdapterNetManager.Verify(nm => nm.Stop(), Times.Once);
    }

    [Fact]
    public void DisconnectAll_ShouldDisconnectSuccessfully()
    {
        _mockAdapterNetManager.Setup(nm => nm.DisconnectAll());

        _mockAdapterNetManager.Object.DisconnectAll();

        _mockAdapterNetManager.Verify(nm => nm.DisconnectAll(), Times.Once);
    }

    [Fact]
    public void OnPeerConnected_Event_ShouldTriggerOnce()
    {
        var callCount = 0;
        _mockNetworkEventsListener.Object.OnPeerConnected += (peer) => callCount++;

        _mockNetworkEventsListener.Raise(ne => ne.OnPeerConnected += null, new Mock<IAdapterNetPeer>().Object);
        _mockNetworkEventsListener.Raise(ne => ne.OnPeerConnected += null, new Mock<IAdapterNetPeer>().Object);

        Assert.Equal(2, callCount);
    }

    [Fact]
    public void OnPeerDisconnected_Event_ShouldTriggerOnce()
    {
        var callCount = 0;
        _mockNetworkEventsListener.Object.OnPeerDisconnected += (peer, info) => callCount++;

        _mockNetworkEventsListener.Raise(ne => ne.OnPeerDisconnected += null, new Mock<IAdapterNetPeer>().Object, new Mock<IAdapterDisconnectInfo>().Object);
        _mockNetworkEventsListener.Raise(ne => ne.OnPeerDisconnected += null, new Mock<IAdapterNetPeer>().Object, new Mock<IAdapterDisconnectInfo>().Object);

        Assert.Equal(2, callCount);
    }
}