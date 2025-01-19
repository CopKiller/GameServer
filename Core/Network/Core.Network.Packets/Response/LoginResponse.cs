using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Response;

namespace Core.Network.Packets.Response;

public class LoginResponse
{
    public ResponseDto Response { get; set; } = new();

    public AccountDto Account { get; set; } = new();
}