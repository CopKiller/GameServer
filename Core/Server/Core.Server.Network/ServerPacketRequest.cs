using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Packet;
using Core.Server.Network.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network;

public class ServerPacketRequest(
    IPacketSender packetSender,
    IServerConnectionManager connectionManager,
    ILogger<ServerPacketRequest> logger) : IServerPacketSender
{

    public void SendPacket<TPacket>(IAdapterNetPeer peer, TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        if (peer.IsConnected)
            packetSender.SendPacket(peer, packet, deliveryMethod);
    }

    public void SendPacket<T>(IAdapterNetPeer peer, ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : IAdapterSerializable
    {
        if (peer.IsConnected)
            packetSender.SendPacket(peer, ref packet, deliveryMethod);
    }

    public void SendPacket(IAdapterNetPeer peer, byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        if (peer.IsConnected)
            packetSender.SendPacket(peer, data, deliveryMethod);
    }

    public void SendPacketToAll<TPacket>(TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        if (connectionManager.HasConnectedPeers)
            packetSender.SendPacketToAll(packet, deliveryMethod);
    }

    public void SendPacketToAll<T>(ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : IAdapterSerializable
    {
        if (connectionManager.HasConnectedPeers)
            packetSender.SendPacketToAll(ref packet, deliveryMethod);
    }

    public void SendPacketToAll(byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        if (connectionManager.HasConnectedPeers)
            packetSender.SendPacketToAll(data, deliveryMethod);
    }
}