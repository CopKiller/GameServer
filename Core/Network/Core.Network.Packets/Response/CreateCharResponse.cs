using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Response;

namespace Core.Network.Packets.Response;

public class CreateCharResponse
{
    public ResponseDto Response { get; set; } = new();

    public PlayerDto Player { get; set; } = new();
}