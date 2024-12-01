using Core.Network.Interface;
using Core.Network.SerializationObjects.Player;

namespace Core.Network.SerializationObjects;

public class PlayerDto : ICustomSerializable
{
    public int Id { get; set; }
    public int SlotNumber { get; set; }
    public string? Name { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Gold { get; set; }
    public VitalsDto? Vitals { get; set; }
    public StatsDto? Stats { get; set; }
    public PositionDto? Position { get; set; }
    
    public void Deserialize(ICustomDataReader reader)
    {
        Id = reader.GetInt();
        SlotNumber = reader.GetInt();
        Name = reader.GetString();
        Level = reader.GetInt();
        Experience = reader.GetInt();
        Gold = reader.GetInt();
        Vitals = reader.Get(() => new VitalsDto());
        Stats = reader.Get(() => new StatsDto());
        Position = reader.Get(() => new PositionDto());
    }
    
    public void Serialize(ICustomDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(SlotNumber);
        writer.Put(Name);
        writer.Put(Level);
        writer.Put(Experience);
        writer.Put(Gold);
        writer.Put(Vitals);
        writer.Put(Stats);
        writer.Put(Position);
    }
}