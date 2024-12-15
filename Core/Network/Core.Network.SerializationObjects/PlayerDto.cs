using Core.Network.Interface;
using Core.Network.SerializationObjects.Player;

namespace Core.Network.SerializationObjects;

public class PlayerDto : ICustomSerializable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SlotNumber { get; set; }
    
    // Ignore in serialization
    public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly LastLogin { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Golds { get; set; }
    public int Diamonds { get; set; }
    public VitalsDto Vitals { get; set; } = new();
    public StatsDto Stats { get; set; } = new();
    public PositionDto Position { get; set; } = new();

    public void Deserialize(ICustomDataReader reader)
    {
        Id = reader.GetInt();
        Name = reader.GetString();
        SlotNumber = reader.GetInt();
        Level = reader.GetInt();
        Experience = reader.GetInt();
        Golds = reader.GetInt();
        Diamonds = reader.GetInt();
        Vitals = reader.Get<VitalsDto>(() => new VitalsDto());
        Stats = reader.Get<StatsDto>(() => new StatsDto());
        Position = reader.Get<PositionDto>(() => new PositionDto());
    }

    public void Serialize(ICustomDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(SlotNumber);
        writer.Put(Name);
        writer.Put(Level);
        writer.Put(Experience);
        writer.Put(Golds);
        writer.Put(Diamonds);
        writer.Put(Vitals);
        writer.Put(Stats);
        writer.Put(Position);
    }
}