using Core.Database.Models.Account;
using Core.Network.Interface;
using Core.Network.Interface.Packet;
using Core.Network.Packets.Handler;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Core.Network.Packets.Response.Interface;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Response;
using Core.Server.Database.Interface;
using Core.Server.Network.Interface;
using Core.Utils.AutoMapper.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network.Packet.Handler;

public class LoginNetHandler(
    IAccountRepository<AccountModel> accountRepository,
    IMapperService mapperService,
    IServerPacketSender sender,
    ILogger<LoginHandler> logger) : LoginHandler
{
    public override async void HandleRequest(LoginRequest request, IAdapterNetPeer peer)
    {
        logger.LogInformation("Login request received in server");
        
        var (validator, account) = await accountRepository.GetAccountAsync(request.Username, request.Password);

        var loginResponse = new LoginResponse();

        if (validator.IsValid == false)
        {
            HandleFailure(loginResponse, peer);
            return;
        }
        
        if (account == null)
        {
            HandleFailure(loginResponse, peer);
            return;
        }
        
        loginResponse.Account = mapperService.Map<AccountModel, AccountDto>(account);
        HandleSuccess(loginResponse, peer);
    }

    public override void HandleSuccess(LoginResponse response, IAdapterNetPeer peer)
    {
        response.Response.Success = true;
        response.Response.Message = "Login successful";
        
        sender.SendPacket(peer, response);
        
    }

    public override void HandleFailure(LoginResponse response, IAdapterNetPeer peer)
    {
        response.Response.Success = false;
        response.Response.Message = "Login failed";
            
        sender.SendPacket(peer, response);
        return;
    }
}