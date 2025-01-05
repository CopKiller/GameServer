using Core.Network.Packets.Response.Interface;

namespace Core.Network.Packets.Response;

public class LoginResponse : IResponse<LoginResponse>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}