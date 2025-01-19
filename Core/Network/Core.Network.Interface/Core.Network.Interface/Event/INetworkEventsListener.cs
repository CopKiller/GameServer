using System.Net;
using System.Net.Sockets;
using Core.Network.Interface.Enum;

namespace Core.Network.Interface.Event;

public interface INetworkEventsListener
{
    event Action<IAdapterNetPeer> OnPeerConnected;
    event Action<IAdapterNetPeer, IAdapterDisconnectInfo> OnPeerDisconnected;
    event Action<IPEndPoint, SocketError> OnNetworkError;
    event Action<IAdapterNetPeer, IAdapterNetPacketReader, byte, CustomDeliveryMethod> OnNetworkReceive;
    event Action<IPEndPoint, IAdapterNetPacketReader, CustomUnconnectedMessageType> OnNetworkReceiveUnconnected;
    event Action<IAdapterNetPeer, int> OnNetworkLatencyUpdate;
    event Action<IAdapterConnectionRequest> OnConnectionRequest;

    void ClearEvents();
    void ClearPeerConnectedEvent();
    void ClearPeerDisconnectedEvent();
    void ClearNetworkErrorEvent();
    void ClearNetworkReceiveEvent();
    void ClearNetworkReceiveUnconnectedEvent();
    void ClearNetworkLatencyUpdateEvent();
    void ClearConnectionRequestEvent();
}