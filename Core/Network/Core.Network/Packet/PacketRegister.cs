using Core.Network.Interface;
using Core.Network.Interface.Packet;
using LiteNetLib.Utils;

namespace Core.Network.Packet;

public class PacketRegister(NetPacketProcessor packetProcessor) : IPacketRegister
{
    public void RegisterPacket<TPacket>(Action<TPacket, ICustomNetPeer> onReceive) where TPacket : class, new()
    {
        packetProcessor.SubscribeReusable(onReceive);
    }
    
    public void UnregisterPackets()
    {
        packetProcessor.ClearSubscriptions();
    }
}