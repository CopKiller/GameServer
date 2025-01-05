namespace Core.Network.Packets.Handler.Interface;

public interface IHandler<in T> where T : class
{
    void HandleRequest(T packet);
    void HandleResponse(T packet);
    void HandleSuccess(T packet);
    void HandleFailure(T packet);
}