using Core.Network.Interface;
using Core.Network.Packets.Client;
using Core.Network.Packets.Server;
using Core.Server.Network.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network;

public class ServerNetworkPacket(ICustomPacketProcessor processor, ILogger<ServerNetworkProcessor> logger)
{
    public void OnFirstPacket(SPacketFirst packet, ICustomNetPeer peer)
    {
        logger.LogInformation($"Server: Received packet: {packet.GetType().Name}");
        
        processor.SendPacket(peer, new CPacketSecond());
    }
    
    public void OnSecondPacket(SPacketSecond packet, ICustomNetPeer peer)
    {
        logger.LogInformation($"Server: Received packet: {packet.GetType().Name}");
        
        processor.SendPacket(peer, new CPacketFirst());
    }
}