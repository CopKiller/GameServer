using Core.Network.DtoObjects.Player;

namespace Core.Network.DtoObjects;

public class PlayerDto
{
    public int Id { get; set; }
    public int SlotNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Gold { get; set; }
    public VitalsDto? Vitals { get; set; }
    public StatsDto? Stats { get; set; }
    public PositionDto? Position { get; set; }
}