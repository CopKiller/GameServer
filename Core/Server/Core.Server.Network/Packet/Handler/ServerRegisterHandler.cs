using Core.Network.Interface;
using Core.Network.Interface.Packet;
using Core.Network.Interface.Serialization;
using Core.Network.Packets.Handler.Interface;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Core.Server.Network.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network.Packet.Handler;

public class ServerRegisterHandler(IHandlerRegistry registry, IServerPacketSender sender, ILoggerFactory loggerFactory)
{
    public void Register()
    {
        
        // Request -> Recebe uma requisição do client para processar aqui no servidor
        registry.RegisterRequestHandler<LoginRequest>(new LoginNetHandler(sender, CreateLogger<LoginNetHandler>()));
    
        // Response -> Recebe uma resposta do client para processar aqui no servidor
    }
    
    private ILogger<T> CreateLogger<T>()
    {
        return loggerFactory.CreateLogger<T>();
    }
}