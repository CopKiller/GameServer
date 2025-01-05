using Core.Network.Interface;
using Core.Network.Interface.Packet;
using Core.Network.Packets.Handler;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Core.Server.Network.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network.Packet.Handler;

public class LoginNetHandler(IServerPacketSender sender, ILogger<LoginHandler> logger) : LoginHandler
{
    public override void HandleRequest(LoginRequest request, IAdapterNetPeer peer)
    {
        logger.LogInformation("Login request received in server");
        
        // Send response to client
        var response = new LoginResponse
        {
            Success = true,
            Message = "Login successful"
        };
        
        sender.SendPacket(peer, response);
    }

    public override void HandleSuccess(LoginResponse response, IAdapterNetPeer peer)
    {
        logger.LogInformation("Login response received in server");
    }

    public override void HandleFailure(LoginResponse response, IAdapterNetPeer peer)
    {
        logger.LogInformation("Login response received in server");
    }
}