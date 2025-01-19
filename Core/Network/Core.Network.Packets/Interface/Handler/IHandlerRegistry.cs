using Core.Network.Packets.Interface.Request;
using Core.Network.Packets.Interface.Response;

namespace Core.Network.Packets.Interface.Handler;

public interface IHandlerRegistry
{
    void RegisterRequestHandler<T>(IHandlerRequest<T> handler) where T : class, new();
    void RegisterResponseHandler<T>(IHandlerResponse<T> handler) where T : class, new();
    void UnregisterRequestHandler<T>() where T : class;
    void UnregisterResponseHandler<T>() where T : class;
    IHandlerRequest<T>? GetRequestHandler<T>() where T : class;
    IHandlerResponse<T>? GetResponseHandler<T>() where T : class;
}