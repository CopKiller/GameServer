namespace Core.Network.Packets.Response.Interface;

public interface IResponse<T>
{
    bool Success { get; set; }
    string Message { get; set; }
}