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

public class RegisterNetHandler(
    IAccountRepository<AccountModel> accountRepository,
    IMapperService mapperService,
    IServerPacketSender sender,
    ILogger<RegisterHandler> logger) : RegisterHandler
{
    public override async void HandleRequest(RegisterRequest request, IAdapterNetPeer peer)
    {
        logger.LogInformation("Register request received in server");
        
        var accountModel = mapperService.Map<AccountDto, AccountModel>(request.Account);

        var (validator, account) = await accountRepository.AddAccountAsync(accountModel);

        var registerResponse = new RegisterResponse();

        if (validator.IsValid == false)
        {
            HandleFailure(registerResponse, peer);
            return;
        }
        
        if (account == null)
        {
            HandleFailure(registerResponse, peer);
            return;
        }
        
        registerResponse.Account = mapperService.Map<AccountModel, AccountDto>(account);
        HandleSuccess(registerResponse, peer);
    }

    public override void HandleSuccess(RegisterResponse response, IAdapterNetPeer peer)
    {
        response.Response = new ResponseDto
        {
            Success = true,
            Message = "Register successful"
        };
        
        sender.SendPacket(peer, response);
    }

    public override void HandleFailure(RegisterResponse response, IAdapterNetPeer peer)
    {
        response.Response = new ResponseDto
        {
            Success = false,
            Message = "Register failed"
        };
            
        sender.SendPacket(peer, response);
        return;
    }
}