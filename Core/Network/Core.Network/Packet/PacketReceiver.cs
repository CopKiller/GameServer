using Core.Network.Interface;
using Core.Network.Interface.Packet;
using LiteNetLib.Utils;

namespace Core.Network.Packet;

public class PacketReceiver(NetPacketProcessor packetProcessor) : IPacketReceiver
{
    public void ReadAllPackets(ICustomNetPacketReader customNetPacketReader, ICustomNetPeer customNetPeer)
    {
        if (customNetPacketReader is not CustomNetPacketReader netPacketReader)
            throw new InvalidOperationException("Invalid customNetPacketReader type. Expected CustomNetPacketReader.");
        
        packetProcessor.ReadAllPackets(netPacketReader.GetReader, customNetPeer);
    }
}