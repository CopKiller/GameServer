using System.Net;
using System.Net.Sockets;
using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface ICustomEventBasedNetListener
{
    event Action<ICustomNetPeer> OnPeerConnected;
    event Action<ICustomNetPeer, ICustomDisconnectInfo> OnPeerDisconnected;
    event Action<IPEndPoint, SocketError> OnNetworkError;
    event Action<ICustomNetPeer, ICustomNetPacketReader, byte, CustomDeliveryMethod> OnNetworkReceive;
    event Action<IPEndPoint, ICustomNetPacketReader, CustomUnconnectedMessageType> OnNetworkReceiveUnconnected;
    event Action<ICustomNetPeer, int> OnNetworkLatencyUpdate;
    event Action<ICustomConnectionRequest> OnConnectionRequest;

    void ClearEvents();
    void ClearPeerConnectedEvent();
    void ClearPeerDisconnectedEvent();
    void ClearNetworkErrorEvent();
    void ClearNetworkReceiveEvent();
    void ClearNetworkReceiveUnconnectedEvent();
    void ClearNetworkLatencyUpdateEvent();
    void ClearConnectionRequestEvent();
}