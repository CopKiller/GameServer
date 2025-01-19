

using Core.Network.SerializationObjects;

namespace Core.Network.Packets.Request;

public class LoginRequest
{
    public AccountDto Account { get; set; } = new();
}