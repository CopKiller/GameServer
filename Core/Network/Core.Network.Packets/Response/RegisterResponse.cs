using Core.Network.Packets.Response.Interface;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Response;

namespace Core.Network.Packets.Response;

public class RegisterResponse
{
    public ResponseDto Response { get; set; } = new();

    public AccountDto Account { get; set; } = new();
}