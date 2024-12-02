using System.Net;
using System.Net.Sockets;
using Core.Network;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;

namespace Core.Network;

public sealed class CustomEventBasedNetListener : ICustomEventBasedNetListener
{
    private readonly EventBasedNetListener _listener;

    public CustomEventBasedNetListener()
    {
        _listener = new EventBasedNetListener();
    }

    // Eventos customizados expostos pela interface
    public event Action<ICustomNetPeer>? OnPeerConnected
    {
        add => _listener.PeerConnectedEvent += peer => value?.Invoke(new CustomNetPeer(peer));
        remove => _listener.PeerConnectedEvent -= peer => value?.Invoke(new CustomNetPeer(peer));
    }

    public event Action<ICustomNetPeer, ICustomDisconnectInfo>? OnPeerDisconnected
    {
        add => _listener.PeerDisconnectedEvent += (peer, disconnectInfo) =>
            value?.Invoke(new CustomNetPeer(peer), new CustomDisconnectInfo(disconnectInfo));
        remove => _listener.PeerDisconnectedEvent -= (peer, disconnectInfo) =>
            value?.Invoke(new CustomNetPeer(peer), new CustomDisconnectInfo(disconnectInfo));
    }

    public event Action<IPEndPoint, SocketError>? OnNetworkError
    {
        add => _listener.NetworkErrorEvent += (endPoint, socketError) =>
            value?.Invoke(endPoint, socketError);
        remove => _listener.NetworkErrorEvent -= (endPoint, socketError) =>
            value?.Invoke(endPoint, socketError);
    }

    public event Action<ICustomNetPeer, ICustomNetPacketReader, byte, CustomDeliveryMethod>? OnNetworkReceive
    {
        add => _listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
            value?.Invoke(new CustomNetPeer(peer), new CustomNetPacketReader(reader), channel,
                Extensions.ConvertToCustomDeliveryMethod(deliveryMethod));
        remove => _listener.NetworkReceiveEvent -= (peer, reader, channel, deliveryMethod) =>
            value?.Invoke(new CustomNetPeer(peer), new CustomNetPacketReader(reader), channel,
                Extensions.ConvertToCustomDeliveryMethod(deliveryMethod));
    }

    public event Action<IPEndPoint, ICustomNetPacketReader, CustomUnconnectedMessageType>? OnNetworkReceiveUnconnected
    {
        add => _listener.NetworkReceiveUnconnectedEvent += (endPoint, reader, messageType) =>
            value?.Invoke(endPoint, new CustomNetPacketReader(reader),
                Extensions.ConvertToCustomUnconnectedMessageType(messageType));
        remove => _listener.NetworkReceiveUnconnectedEvent -= (endPoint, reader, messageType) =>
            value?.Invoke(endPoint, new CustomNetPacketReader(reader),
                Extensions.ConvertToCustomUnconnectedMessageType(messageType));
    }

    public event Action<ICustomNetPeer, int>? OnNetworkLatencyUpdate
    {
        add => _listener.NetworkLatencyUpdateEvent += (peer, latency) =>
            value?.Invoke(new CustomNetPeer(peer), latency);
        remove => _listener.NetworkLatencyUpdateEvent -= (peer, latency) =>
            value?.Invoke(new CustomNetPeer(peer), latency);
    }

    public event Action<ICustomConnectionRequest>? OnConnectionRequest
    {
        add => _listener.ConnectionRequestEvent += request => value?.Invoke(new CustomConnectionRequest(request));
        remove => _listener.ConnectionRequestEvent -= request => value?.Invoke(new CustomConnectionRequest(request));
    }

    public void ClearEvents()
    {
        ClearPeerConnectedEvent();
        ClearPeerDisconnectedEvent();
        ClearNetworkErrorEvent();
        ClearNetworkReceiveEvent();
        ClearNetworkReceiveUnconnectedEvent();
        ClearNetworkLatencyUpdateEvent();
        ClearConnectionRequestEvent();
    }

    // Métodos Clear mantêm a mesma funcionalidade
    public void ClearPeerConnectedEvent() => _listener.ClearPeerConnectedEvent();
    public void ClearPeerDisconnectedEvent() => _listener.ClearPeerDisconnectedEvent();
    public void ClearNetworkErrorEvent() => _listener.ClearNetworkErrorEvent();
    public void ClearNetworkReceiveEvent() => _listener.ClearNetworkReceiveEvent();
    public void ClearNetworkReceiveUnconnectedEvent() => _listener.ClearNetworkReceiveUnconnectedEvent();
    public void ClearNetworkLatencyUpdateEvent() => _listener.ClearNetworkLatencyUpdateEvent();
    public void ClearConnectionRequestEvent() => _listener.ClearConnectionRequestEvent();

    internal EventBasedNetListener GetListener() => _listener;
}
