using System.Net;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;

namespace Core.Network;

public sealed class CustomNetPeer : ICustomNetPeer
{
    public readonly NetPeer Peer;

    public CustomNetPeer(NetPeer peer)
    {
        Peer = peer;
    }

    public int Id => Peer.Id;
    
    public bool IsConnected => Peer.ConnectionState == ConnectionState.Connected;
    
    public IPAddress EndPoint => Peer.Address;

    public void Send(byte[] data, CustomDeliveryMethod deliveryMethod)
    {
        // Traduzir o CustomDeliveryMethod para DeliveryMethod do LiteNetLib
        DeliveryMethod liteDeliveryMethod = deliveryMethod switch
        {
            CustomDeliveryMethod.ReliableOrdered => DeliveryMethod.ReliableOrdered,
            CustomDeliveryMethod.ReliableUnordered => DeliveryMethod.ReliableUnordered,
            CustomDeliveryMethod.Unreliable => DeliveryMethod.Unreliable,
            _ => throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null)
        };

        Peer.Send(data, liteDeliveryMethod);
    }
}