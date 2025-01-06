using Core.Database.Models.Account;
using Core.Network.Packets.Handler.Interface;
using Core.Network.Packets.Request;
using Core.Server.Database.Interface;
using Core.Server.Network.Interface;
using Core.Server.Network.Packet.Handler;
using Core.Utils.AutoMapper.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network.Packet;

public class ServerRegisterHandler(
    IHandlerRegistry registry,
    IDatabaseService databaseService,
    IMapperService mapperService,
    IServerPacketSender sender, 
    ILoggerFactory loggerFactory)
{
    public void Register()
    {
        // Request -> Recebe uma requisição do client para processar aqui no servidor
        registry.RegisterRequestHandler<LoginRequest>(
            new LoginNetHandler(
                databaseService.GetAccountRepository(), 
                mapperService,
                sender, 
                CreateLogger<LoginNetHandler>()));
        
        registry.RegisterRequestHandler<RegisterRequest>(
            new RegisterNetHandler(
                databaseService.GetAccountRepository(), 
                mapperService,
                sender, 
                CreateLogger<RegisterNetHandler>()));
    
        // Response -> Recebe uma resposta do client para processar aqui no servidor
    }
    
    private ILogger<T> CreateLogger<T>()
    {
        return loggerFactory.CreateLogger<T>();
    }
}