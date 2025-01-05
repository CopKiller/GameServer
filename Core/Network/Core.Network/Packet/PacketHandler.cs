using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Event;
using Core.Network.Interface.Packet;
using LiteNetLib.Utils;

namespace Core.Network.Packet;

public class PacketHandler : IPacketHandler
{
    private readonly NetPacketProcessor _packetProcessor;
    public PacketHandler(NetPacketProcessor packetProcessor, INetworkEventsListener networkEventsListener)
    {
        _packetProcessor = packetProcessor;
        networkEventsListener.OnNetworkReceive += ReadAllPackets;
    }
    
    public void ReadAllPackets(IAdapterNetPeer adapterNetPeer, IAdapterNetPacketReader adapterNetPacketReader, byte channel, CustomDeliveryMethod deliveryMethod)
    {
        if (adapterNetPacketReader is not AdapterNetPacketReader netPacketReader)
            throw new InvalidOperationException("Invalid customNetPacketReader type. Expected CustomNetPacketReader.");

        _packetProcessor.ReadAllPackets(netPacketReader.GetReader, adapterNetPeer);
    }
}