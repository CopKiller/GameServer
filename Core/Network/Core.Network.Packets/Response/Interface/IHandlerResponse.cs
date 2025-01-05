namespace Core.Network.Packets.Response.Interface;

public interface IHandlerResponse <in T> where T : class
{
    void HandleResponse(T packet);
    void HandleSuccess(T packet);
    void HandleFailure(T packet);
}