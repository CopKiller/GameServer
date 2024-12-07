using Core.Network.SerializationObjects;

namespace Core.Network.Packets.Server;

public class SPacketSecond
{
    public PlayerDto Player { get; set; } = new();
}