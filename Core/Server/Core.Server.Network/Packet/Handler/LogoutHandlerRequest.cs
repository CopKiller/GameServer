using Core.Network.Interface;
using Core.Network.Packets.Interface.Request;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Core.Network.SerializationObjects.Enum;
using Core.Server.Network.Interface;
using Core.Server.Session;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network.Packet.Handler;

public class LogoutHandlerRequest(
    ISessionManager sessionManager,
    IServerPacketSender packetSender,
    ILogger<LogoutHandlerRequest> logger) : IHandlerRequest<LogoutRequest>
{
    public void HandleRequest(LogoutRequest packet, IAdapterNetPeer peer)
    {
        sessionManager.LogoutAccountSession(peer.Id);
        
        logger.LogInformation($"Account session {peer.Id} logged out.");
        
        // Send response
        // var response = new LogoutResponse
        // {
        //     ClientState = ClientState.MainMenu
        // };
        //
        // packetSender.SendPacket(peer, response);
    }
}