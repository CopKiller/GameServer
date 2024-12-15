namespace Core.Database.Interfaces.Player;

public interface IPlayerModel : IEntity
{
    string Name { get; set; }
    byte SlotNumber { get; set; }
    DateOnly CreatedAt { get; set; }
    DateOnly LastLogin { get; set; }
    int Level { get; set; }
    int Experience { get; set; }
    int Golds { get; set; }
    int Diamonds { get; set; }
    IVitals Vitals { get; set; }
    IStats Stats { get; set; }
    IPosition Position { get; set; }

    int AccountModelId { get; set; }
}