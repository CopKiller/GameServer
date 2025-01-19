using System.Net.Sockets;
using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface IAdapterDisconnectInfo
{
    CustomDisconnectReason Reason { get; }

    SocketError SocketErrorCode { get; }
}