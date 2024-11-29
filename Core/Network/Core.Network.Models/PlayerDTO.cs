using Core.Network.Models.Player;

namespace Core.Network.Models;

public class PlayerDTO
{
    public int Id { get; set; }
    public int SlotNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Gold { get; set; }
    public VitalsDTO? Vitals { get; set; }
    public StatsDTO? Stats { get; set; }
    public PositionDTO? Position { get; set; }
}