using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Packet;
using LiteNetLib.Utils;

namespace Core.Network.Packet;

public class PacketSender(NetPacketProcessor packetProcessor, IConnectionManager manager) : IPacketSender
{
    public void SendPacket<TPacket>(ICustomNetPeer peer, TPacket packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        if (peer is not CustomNetPeer liteNetPeer)
            throw new InvalidOperationException("Invalid peer type. Expected CustomNetPeer.");

        packetProcessor.Send(liteNetPeer.Peer, packet, Extensions.ConvertToLiteDeliveryMethod(deliveryMethod));
    }
    
    public void SendPacket<T>(ICustomNetPeer peer, ref T packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    {
        if (peer is not CustomNetPeer liteNetPeer)
            throw new InvalidOperationException("Invalid peer type. Expected CustomNetPeer.");

        var adapter = new LiteNetSerializableAdapter<T>(packet);
        packetProcessor.SendNetSerializable(liteNetPeer.Peer, ref adapter, Extensions.ConvertToLiteDeliveryMethod(deliveryMethod));
    }
    
    public void SendPacket(ICustomNetPeer peer, byte[] data, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        peer.Send(data, deliveryMethod);
    }
    
    public void SendPacketToAll<TPacket>(TPacket packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        var allPeers = manager.CustomPeers.Values;
        
        foreach (var peer in allPeers)
        {
            SendPacket(peer, packet, deliveryMethod);
        }
    }
    
    public void SendPacketToAll<T>(ref T packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    {
        
        var allPeers = manager.CustomPeers.Values;
        
        var adapter = new LiteNetSerializableAdapter<T>(packet);
        
        foreach (var peer in allPeers)
        {
            SendPacket(peer, adapter, deliveryMethod);
        }
    }
    
    public void SendPacketToAll(byte[] data, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        var allPeers = manager.CustomPeers.Values;
        
        foreach (var peer in allPeers)
        {
            peer.Send(data, deliveryMethod);
        }
    }
}