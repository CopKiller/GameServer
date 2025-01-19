

using Core.Network.SerializationObjects;

namespace Core.Network.Packets.Request;

public class CreateCharRequest
{
    public PlayerDto Player { get; set; } = new();
}