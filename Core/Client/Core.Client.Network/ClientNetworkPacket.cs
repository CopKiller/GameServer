using Core.Network.Interface;
using Core.Network.Packets.Client;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network;

public class ClientNetworkPacket(ILogger<ClientPacketProcessor> logger)
{
    public void OnFirstPacket(CPacketFirst packet, ICustomNetPeer peer)
    {
        logger.LogInformation($"Client: Received packet: {packet.GetType().Name}");
    }

    public void OnSecondPacket(CPacketSecond packet, ICustomNetPeer peer)
    {
        logger.LogInformation($"Client: Received packet: {packet.GetType().Name}");
    }
}