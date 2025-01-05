using System.Net;
using System.Net.Sockets;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Event;
using LiteNetLib;

namespace Core.Network.Event;

public sealed class NetworkEventsListener(EventBasedNetListener listener) : INetworkEventsListener
{
    public event Action<IAdapterNetPeer>? OnPeerConnected
    {
        add => listener.PeerConnectedEvent += peer => value?.Invoke(new AdapterNetPeer(peer));
        remove => listener.PeerConnectedEvent -= peer => value?.Invoke(new AdapterNetPeer(peer));
    }

    public event Action<IAdapterNetPeer, IAdapterDisconnectInfo>? OnPeerDisconnected
    {
        add => listener.PeerDisconnectedEvent += (peer, disconnectInfo) =>
            value?.Invoke(new AdapterNetPeer(peer), new AdapterDisconnectInfo(disconnectInfo));
        remove => listener.PeerDisconnectedEvent -= (peer, disconnectInfo) =>
            value?.Invoke(new AdapterNetPeer(peer), new AdapterDisconnectInfo(disconnectInfo));
    }

    public event Action<IPEndPoint, SocketError>? OnNetworkError
    {
        add => listener.NetworkErrorEvent += (endPoint, socketError) =>
            value?.Invoke(endPoint, socketError);
        remove => listener.NetworkErrorEvent -= (endPoint, socketError) =>
            value?.Invoke(endPoint, socketError);
    }

    public event Action<IAdapterNetPeer, IAdapterNetPacketReader, byte, CustomDeliveryMethod>? OnNetworkReceive
    {
        add => listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
            value?.Invoke(new AdapterNetPeer(peer), new AdapterNetPacketReader(reader), channel,
                Extensions.ConvertToCustomDeliveryMethod(deliveryMethod));
        remove => listener.NetworkReceiveEvent -= (peer, reader, channel, deliveryMethod) =>
            value?.Invoke(new AdapterNetPeer(peer), new AdapterNetPacketReader(reader), channel,
                Extensions.ConvertToCustomDeliveryMethod(deliveryMethod));
    }

    public event Action<IPEndPoint, IAdapterNetPacketReader, CustomUnconnectedMessageType>? OnNetworkReceiveUnconnected
    {
        add => listener.NetworkReceiveUnconnectedEvent += (endPoint, reader, messageType) =>
            value?.Invoke(endPoint, new AdapterNetPacketReader(reader),
                Extensions.ConvertToCustomUnconnectedMessageType(messageType));
        remove => listener.NetworkReceiveUnconnectedEvent -= (endPoint, reader, messageType) =>
            value?.Invoke(endPoint, new AdapterNetPacketReader(reader),
                Extensions.ConvertToCustomUnconnectedMessageType(messageType));
    }

    public event Action<IAdapterNetPeer, int>? OnNetworkLatencyUpdate
    {
        add => listener.NetworkLatencyUpdateEvent += (peer, latency) =>
            value?.Invoke(new AdapterNetPeer(peer), latency);
        remove => listener.NetworkLatencyUpdateEvent -= (peer, latency) =>
            value?.Invoke(new AdapterNetPeer(peer), latency);
    }

    public event Action<IAdapterConnectionRequest>? OnConnectionRequest
    {
        add => listener.ConnectionRequestEvent += request => value?.Invoke(new AdapterConnectionRequest(request));
        remove => listener.ConnectionRequestEvent -= request => value?.Invoke(new AdapterConnectionRequest(request));
    }

    public void ClearPeerConnectedEvent()
    {
        listener.ClearPeerConnectedEvent();
    }

    public void ClearPeerDisconnectedEvent()
    {
        listener.ClearPeerDisconnectedEvent();
    }

    public void ClearNetworkErrorEvent()
    {
        listener.ClearNetworkErrorEvent();
    }

    public void ClearNetworkReceiveEvent()
    {
        listener.ClearNetworkReceiveEvent();
    }

    public void ClearNetworkReceiveUnconnectedEvent()
    {
        listener.ClearNetworkReceiveUnconnectedEvent();
    }

    public void ClearNetworkLatencyUpdateEvent()
    {
        listener.ClearNetworkLatencyUpdateEvent();
    }

    public void ClearConnectionRequestEvent()
    {
        listener.ClearConnectionRequestEvent();
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
}