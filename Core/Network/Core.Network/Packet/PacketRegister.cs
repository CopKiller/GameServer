using Core.Network.Interface;
using Core.Network.Interface.Packet;
using LiteNetLib.Utils;

namespace Core.Network.Packet;

public class PacketRegister(NetPacketProcessor packetProcessor) : IPacketRegister
{
    private readonly HashSet<Type> nestedTypes = new();
    public void RegisterPacket<TPacket>(Action<TPacket, IAdapterNetPeer> onReceive) where TPacket : class, new()
    {
        if (nestedTypes.Contains(typeof(TPacket)))
        {
            return;
        }
        
        nestedTypes.Add(typeof(TPacket));
        
        packetProcessor.SubscribeReusable(onReceive);
    }
    
    public void UnregisterPacket<TPacket>()
    {
        packetProcessor.RemoveSubscription<TPacket>();
    }

    public void UnregisterPackets()
    {
        packetProcessor.ClearSubscriptions();
    }
}