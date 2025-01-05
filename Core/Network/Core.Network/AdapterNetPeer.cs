using System.Net;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;

namespace Core.Network;

public sealed class AdapterNetPeer(NetPeer peer) : IAdapterNetPeer
{
    public readonly NetPeer Peer = peer;

    public int Id => Peer.Id;

    public bool IsConnected => Peer.ConnectionState == ConnectionState.Connected;

    public IPAddress EndPoint => Peer.Address;

    public void Send(byte[] data, CustomDeliveryMethod deliveryMethod)
    {
        // Traduzir o CustomDeliveryMethod para DeliveryMethod do LiteNetLib
        var liteDeliveryMethod = deliveryMethod switch
        {
            CustomDeliveryMethod.ReliableOrdered => DeliveryMethod.ReliableOrdered,
            CustomDeliveryMethod.ReliableUnordered => DeliveryMethod.ReliableUnordered,
            CustomDeliveryMethod.Unreliable => DeliveryMethod.Unreliable,
            _ => throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null)
        };

        Peer.Send(data, liteDeliveryMethod);
    }

    public void Send(IAdapterDataWriter writer, CustomDeliveryMethod deliveryMethod)
    {
        Send(writer.CopyData(), deliveryMethod);
    }
}