
using Core.Network.Packets.Handler.Interface;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Game.Scripts.Network.Handler;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network;

public class ClientRegisterHandler(IHandlerRegistry registry, ILoggerFactory loggerFactory)
{
    public void Register()
    {
        
        registry.RegisterResponseHandler<LoginResponse>(new LoginNetHandler(CreateLogger<LoginNetHandler>()));
        registry.RegisterResponseHandler<RegisterResponse>(new RegisterNetHandler(CreateLogger<RegisterNetHandler>()));

    }
    
    private ILogger<T> CreateLogger<T>()
    {
        return loggerFactory.CreateLogger<T>();
    }
}