namespace Core.Network.Packets.Response.Interface;

public interface IResponse
{
    bool Success { get; set; }
    string Message { get; set; }
}