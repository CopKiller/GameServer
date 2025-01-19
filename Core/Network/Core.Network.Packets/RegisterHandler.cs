using Core.Network.Interface.Packet;
using Core.Network.Packets.Interface.Handler;
using Core.Network.Packets.Interface.Request;
using Core.Network.Packets.Interface.Response;

namespace Core.Network.Packets;

public class RegisterHandler(IPacketRegister packetRegister) : IHandlerRegistry
{
    private readonly Dictionary<Type, object> _requestHandlers = new();
    private readonly Dictionary<Type, object> _responseHandlers = new();

    public void RegisterRequestHandler<T>(IHandlerRequest<T> handler) where T : class, new()
    {
        _requestHandlers[typeof(T)] = handler;
        
        packetRegister.RegisterPacket<T>(handler.HandleRequest);
    }

    public void RegisterResponseHandler<T>(IHandlerResponse<T> handler) where T : class, new()
    {
        _responseHandlers[typeof(T)] = handler;
        
        packetRegister.RegisterPacket<T>(handler.HandleResponse);
    }

    public void UnregisterRequestHandler<T>() where T : class
    {
        _requestHandlers.Remove(typeof(T));
        packetRegister.UnregisterPacket<T>();
    }

    public void UnregisterResponseHandler<T>() where T : class
    {
        _requestHandlers.Remove(typeof(T));
        packetRegister.UnregisterPacket<T>();
    }

    public IHandlerRequest<T>? GetRequestHandler<T>() where T : class
    {
        _requestHandlers.TryGetValue(typeof(T), out var handler);
        return handler as IHandlerRequest<T>;
    }

    public IHandlerResponse<T>? GetResponseHandler<T>() where T : class
    {
        _responseHandlers.TryGetValue(typeof(T), out var handler);
        return handler as IHandlerResponse<T>;
    }
}