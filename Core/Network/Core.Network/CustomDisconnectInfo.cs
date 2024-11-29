using System.Net.Sockets;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;

namespace Core.Network;

public readonly struct CustomDisconnectInfo(DisconnectInfo disconnectInfo) : ICustomDisconnectInfo
{
    public CustomDisconnectReason Reason => (CustomDisconnectReason)disconnectInfo.Reason;
    
    public SocketError SocketErrorCode => disconnectInfo.SocketErrorCode;

    // TODO: Implement this property if needed...
    //public NetPacketReader AdditionalData => disconnectInfo.AdditionalData;
}