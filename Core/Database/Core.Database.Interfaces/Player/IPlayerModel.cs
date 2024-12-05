namespace Core.Database.Interfaces.Player;

public interface IPlayerModel : IEntity
{
    public int SlotNumber { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Gold { get; set; }
    public IVitals Vitals { get; set; }
    public IStats Stats { get; set; }
    public IPosition Position { get; set; }

    int AccountModelId { get; set; }

    public string ToString()
    {
        return
            $"SlotNumber: {SlotNumber}, Name: {Name}, Level: {Level}, Experience: {Experience}, Gold: {Gold}, Vitals: {Vitals.ToString()}, Stats: {Stats.ToString()}, Position: {Position.ToString()}";
    }
}