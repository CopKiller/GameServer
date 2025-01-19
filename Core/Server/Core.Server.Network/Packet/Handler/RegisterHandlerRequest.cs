using Core.Database.Models.Account;
using Core.Network.Interface;
using Core.Network.Packets;
using Core.Network.Packets.Interface.Request;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Response;
using Core.Server.Database.Interface;
using Core.Server.Network.Interface;
using Core.Utils.AutoMapper.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network.Packet.Handler;

public class RegisterHandlerRequest(
    IAccountRepository<AccountModel> accountRepository,
    IMapperService mapperService,
    IServerPacketSender sender,
    ILogger<RegisterRequest> logger) : IHandlerRequest<RegisterRequest>
{
    public async void HandleRequest(RegisterRequest request, IAdapterNetPeer peer)
    {
        logger.LogInformation("Register request received in server");
        
        var accountModel = mapperService.Map<AccountDto, AccountModel>(request.Account);

        var (validator, account) = await accountRepository.AddAccountAsync(accountModel);

        var registerResponse = new RegisterResponse();

        if (validator.IsValid == false || account == null)
        {
            registerResponse.Response.Success = false;
            registerResponse.Response.Message = string.Join(",\n", validator.Errors);
            
            sender.SendPacket(peer, registerResponse);
            return;
        }
        
        registerResponse.Account = mapperService.Map<AccountModel, AccountDto>(account);
        
        registerResponse.Response.Success = true;
        registerResponse.Response.Message = "Account created successfully";
    }
}