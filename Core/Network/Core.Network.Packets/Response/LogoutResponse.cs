using Core.Network.SerializationObjects.Enum;
using Core.Network.SerializationObjects.Response;

namespace Core.Network.Packets.Response;

public class LogoutResponse
{
    public ResponseDto Response { get; set; } = new();
    public ClientState ClientState { get; set; }
    
}