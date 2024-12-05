namespace Core.Network.Interface.Packet;

public interface IPacketReceiver
{
    void ReadAllPackets(ICustomNetPacketReader customNetPacketReader, ICustomNetPeer customNetPeer);
}