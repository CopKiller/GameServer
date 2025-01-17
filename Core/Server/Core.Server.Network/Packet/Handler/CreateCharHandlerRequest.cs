
using Core.Database.Models.Player;
using Core.Network.Interface;
using Core.Network.Packets;
using Core.Network.Packets.Interface.Request;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Enum;
using Core.Server.Database.Interface;
using Core.Server.Network.Interface;
using Core.Server.Session;
using Core.Utils.AutoMapper.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network.Packet.Handler;

public class CreateCharHandlerRequest(
    IPlayerRepository<PlayerModel> playerRepository,
    IMapperService mapperService,
    IServerPacketSender sender,
    ISessionManager sessionManager,
    ILogger<CreateCharRequest> logger) : IHandlerRequest<CreateCharRequest>
{
    public async void HandleRequest(CreateCharRequest request, IAdapterNetPeer peer)
    {
        var response = new CreateCharResponse();
        
        var getAccountSession = sessionManager.GetAccountSession(peer.Id);
        
        if (getAccountSession?.IsLoggedIn != true || 
            getAccountSession.ClientState != ClientState.CharacterSelection || 
            getAccountSession.CurrentAccount == null)
        {
            response.Response.Success = false;
            response.Response.Message = "Invalid account state.";
            sender.SendPacket(peer, response);
            logger.LogWarning($"Failed to create player for peer {peer.Id}. Reason: {response.Response.Message}");
            return;
        }

        var playerModel = mapperService.Map<PlayerModel>(request.Player);
        var (validatorResult, player) = await playerRepository.AddPlayerAsync(playerModel, getAccountSession.CurrentAccount.Id);
        
        if (!validatorResult.IsValid || player == null)
        {
            response.Response.Success = false;
            response.Response.Message = string.Join(", ", validatorResult.Errors);
            sender.SendPacket(peer, response);
            logger.LogWarning($"Failed to create player for peer {peer.Id}. Reason: {response.Response.Message}");
            return;
        }
        
        getAccountSession.CurrentAccount.Players.Add(mapperService.Map<PlayerDto>(player));

        response.Player = mapperService.Map<PlayerDto>(player);
        response.Response.Success = true;
        
        sender.SendPacket(peer, response);
        logger.LogInformation($"Player '{response.Player.Name}' successfully created for peer {peer.Id}.");
    }
}