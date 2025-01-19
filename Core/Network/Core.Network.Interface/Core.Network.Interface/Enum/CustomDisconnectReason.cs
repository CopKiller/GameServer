namespace Core.Network.Interface.Enum;

public enum CustomDisconnectReason
{
    ConnectionFailed,
    Timeout,
    HostUnreachable,
    NetworkUnreachable,
    RemoteConnectionClose,
    DisconnectPeerCalled,
    ConnectionRejected,
    InvalidProtocol,
    UnknownHost,
    Reconnect,
    PeerToPeerConnection,
    PeerNotFound
}