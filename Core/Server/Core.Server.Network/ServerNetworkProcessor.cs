using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Packets.Server;
using Core.Server.Network.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network;

public class ServerNetworkProcessor(
    ICustomPacketProcessor packetProcessor, 
    ICustomEventBasedNetListener netListener,
    IConnectionManager connectionManager,
    ILogger<ServerNetworkProcessor> logger)
{
    private readonly ServerNetworkPacket _serverNetworkPacket = new();
    
    public void Initialize()
    {
        packetProcessor.RegisterPacket<SPacketFirst>(_serverNetworkPacket.OnFirstPacket);
        packetProcessor.RegisterPacket<SPacketSecond>(_serverNetworkPacket.OnSecondPacket);
        
        netListener.OnNetworkReceive += ProcessPacket;
        netListener.OnConnectionRequest += OnConnectionRequest;
        
        if (connectionManager is not ConnectionManager manager)
        {
            throw new ArgumentException("connectionManager must be an instance of ConnectionManager");
        }
        // connection Manager
        netListener.OnPeerConnected += manager.AddPeer;
        netListener.OnPeerDisconnected += manager.RemovePeer;
    }
    
    private void ProcessPacket(ICustomNetPeer peer, ICustomNetPacketReader reader, byte channel, CustomDeliveryMethod deliveryMethod)
    {
        packetProcessor.ReadAllPackets(reader, peer);
    }
    
    private void OnConnectionRequest(ICustomConnectionRequest request)
    {
        logger.LogInformation($"Connection request from {request.RemoteEndPoint}");
        
        request.AcceptIfKey("key");
    }
}