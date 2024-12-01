using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;
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
        
        _listener.PeerConnectedEvent += peer => OnPeerConnected?.Invoke(new CustomNetPeer(peer));
        _listener.PeerDisconnectedEvent += (peer, disconnectInfo) => OnPeerDisconnected?.Invoke(new CustomNetPeer(peer), new CustomDisconnectInfo(disconnectInfo));
        _listener.NetworkErrorEvent += (endPoint, socketError) => OnNetworkError?.Invoke(endPoint, socketError);
        _listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) => OnNetworkReceive?.Invoke(new CustomNetPeer(peer), new CustomNetPacketReader(reader), channel, Extensions.ConvertToCustomDeliveryMethod(deliveryMethod));
        _listener.NetworkReceiveUnconnectedEvent += (endPoint, reader, messageType) => OnNetworkReceiveUnconnected?.Invoke(endPoint, new CustomNetPacketReader(reader), Extensions.ConvertToCustomUnconnectedMessageType(messageType));
        _listener.NetworkLatencyUpdateEvent += (peer, latency) => OnNetworkLatencyUpdate?.Invoke(new CustomNetPeer(peer), latency);
        _listener.ConnectionRequestEvent += request => OnConnectionRequest?.Invoke(new CustomConnectionRequest(request));
    }
    
    public event Action<ICustomNetPeer>? OnPeerConnected;
    public event Action<ICustomNetPeer, ICustomDisconnectInfo>? OnPeerDisconnected;
    public event Action<IPEndPoint, SocketError>? OnNetworkError;
    public event Action<ICustomNetPeer, ICustomNetPacketReader, byte, CustomDeliveryMethod>? OnNetworkReceive;
    public event Action<IPEndPoint, ICustomNetPacketReader, CustomUnconnectedMessageType>? OnNetworkReceiveUnconnected;
    public event Action<ICustomNetPeer, int>? OnNetworkLatencyUpdate;
    public event Action<ICustomConnectionRequest>? OnConnectionRequest;
    public void ClearEvents()
    {
        OnPeerConnected = null;
        OnPeerDisconnected = null;
        OnNetworkError = null;
        OnNetworkReceive = null;
        OnNetworkReceiveUnconnected = null;
        OnNetworkLatencyUpdate = null;
        OnConnectionRequest = null;
        
        ClearPeerConnectedEvent();
        ClearPeerDisconnectedEvent();
        ClearNetworkErrorEvent();
        ClearNetworkReceiveEvent();
        ClearNetworkReceiveUnconnectedEvent();
        ClearNetworkLatencyUpdateEvent();
        ClearConnectionRequestEvent();
    }
    
    public void ClearPeerConnectedEvent()
    {
        _listener.ClearPeerConnectedEvent();
    }
    
    public void ClearPeerDisconnectedEvent()
    {
        _listener.ClearPeerDisconnectedEvent();
    }
    
    public void ClearNetworkErrorEvent()
    {
        _listener.ClearNetworkErrorEvent();
    }
    
    public void ClearNetworkReceiveEvent()
    {
        _listener.ClearNetworkReceiveEvent();
    }
    
    public void ClearNetworkReceiveUnconnectedEvent()
    {
        _listener.ClearNetworkReceiveUnconnectedEvent();
    }
    
    public void ClearNetworkLatencyUpdateEvent()
    {
        _listener.ClearNetworkLatencyUpdateEvent();
    }
    
    public void ClearConnectionRequestEvent()
    {
        _listener.ClearConnectionRequestEvent();
    }
    
    internal EventBasedNetListener GetListener() => _listener;
}