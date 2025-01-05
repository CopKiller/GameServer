using Core.Network.Packets.Request.Interface;
using Core.Network.Packets.Response.Interface;

namespace Core.Network.Packets.Handler.Interface;

public interface IHandlerRegistry
{
    void RegisterRequestHandler<T>(IHandlerRequest<T> handler) where T : class;
    void RegisterResponseHandler<T>(IHandlerResponse<T> handler) where T : class;
    void UnregisterRequestHandler<T>() where T : class;
    void UnregisterResponseHandler<T>() where T : class;
    IHandlerRequest<T>? GetRequestHandler<T>() where T : class;
    IHandlerResponse<T>? GetResponseHandler<T>() where T : class;
}