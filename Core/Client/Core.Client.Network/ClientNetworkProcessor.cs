using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Packets.Client;

namespace Core.Client.Network;

public class ClientNetworkProcessor(ICustomPacketProcessor packetProcessor, ICustomEventBasedNetListener netListener)
{
    private readonly ClientNetworkPacket _clientNetworkPacket = new();
    
    public void Initialize()
    {
        packetProcessor.RegisterPacket<CPacketFirst>(_clientNetworkPacket.OnFirstPacket);
        packetProcessor.RegisterPacket<CPacketSecond>(_clientNetworkPacket.OnSecondPacket);
        
        netListener.OnNetworkReceive += ProcessPacket;
    }
    
    private void ProcessPacket(ICustomNetPeer peer, ICustomNetPacketReader reader, byte channel, CustomDeliveryMethod deliveryMethod)
    {
        packetProcessor.ReadAllPackets(reader, peer);
    }
}