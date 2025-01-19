

using Core.Network.SerializationObjects;

namespace Core.Network.Packets.Request;

public class RegisterRequest
{
    public AccountDto Account { get; set; } = new();
}