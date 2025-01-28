using Core.Network.Interface;
using Core.Network.SerializationObjects.Player;

namespace Core.Network.SerializationObjects;

public class PlayerDto : IAdapterSerializable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte SlotNumber { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Golds { get; set; }
    public int Diamonds { get; set; }
    public VitalsDto Vitals { get; set; } = new();
    public StatsDto Stats { get; set; } = new();
    public Vector2 Position { get; set; } = new();

    public void Deserialize(IAdapterDataReader reader)
    {
        Id = reader.GetInt();
        Name = reader.GetString();
        SlotNumber = reader.GetByte();
        Level = reader.GetInt();
        Experience = reader.GetInt();
        Golds = reader.GetInt();
        Diamonds = reader.GetInt();
    }

    public void Serialize(IAdapterDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(Name);
        writer.Put(SlotNumber);
        writer.Put(Level);
        writer.Put(Experience);
        writer.Put(Golds);
        writer.Put(Diamonds);
    }
}