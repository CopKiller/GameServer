using Core.Database.Models.Account;
using Core.Network.Interface;
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

public class LoginHandlerRequest(
    IAccountRepository<AccountModel> accountRepository,
    IMapperService mapperService,
    IServerPacketSender sender,
    ISessionManager sessionManager,
    ILogger<LoginRequest> logger) : IHandlerRequest<LoginRequest>
{
    public async void HandleRequest(LoginRequest request, IAdapterNetPeer peer)
    {
        logger.LogInformation("Login request received in server");
        
        var (validator, account) = await accountRepository.GetAccountAsync(request.Account.Username, request.Account.Password);

        var loginResponse = new LoginResponse();

        if (validator.IsValid == false || account == null)
        {
            loginResponse.Response.Success = false;
            loginResponse.Response.Message = string.Join(",\n", validator.Errors);
            sender.SendPacket(peer, loginResponse);
            return;
        }
        
        var hasSession = sessionManager.HasAccountSession(account.Id);
        if (hasSession)
        {
            loginResponse.Response.Success = false;
            loginResponse.Response.Message = "Account is already online";
            sender.SendPacket(peer, loginResponse);
            return;
        }
        
        loginResponse.Account = mapperService.Map<AccountModel, AccountDto>(account);
        
        sessionManager.AddAccountSession(peer, loginResponse.Account, ClientState.CharacterSelection);
        
        loginResponse.Response.Success = true;
        loginResponse.Response.Message = "Login successful";
        
        sender.SendPacket(peer, loginResponse);
    }
}