using Core.Network.Packets.Handler.Interface;
using Core.Network.Packets.Request.Interface;
using Core.Network.Packets.Response.Interface;

namespace Core.Network.Packets;

public class RegisterHandler : IHandlerRegistry
{
    private readonly Dictionary<Type, object> _requestHandlers = new();
    private readonly Dictionary<Type, object> _responseHandlers = new();

    public void RegisterRequestHandler<T>(IHandlerRequest<T> handler) where T : class
    {
        _requestHandlers[typeof(T)] = handler;
    }

    public void RegisterResponseHandler<T>(IHandlerResponse<T> handler) where T : class
    {
        _responseHandlers[typeof(T)] = handler;
    }

    public void UnregisterRequestHandler<T>() where T : class
    {
        _requestHandlers.Remove(typeof(T));
    }

    public void UnregisterResponseHandler<T>() where T : class
    {
        _responseHandlers.Remove(typeof(T));
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