using Core.Network.SerializationObjects;

namespace Core.Network.Packets.Client;

public class CPacketSecond
{
    public PlayerDto Player { get; set; } = new PlayerDto();
}