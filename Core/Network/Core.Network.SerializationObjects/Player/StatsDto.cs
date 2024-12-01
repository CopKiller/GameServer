using Core.Network.Interface;

namespace Core.Network.SerializationObjects.Player;

public class StatsDto : ICustomSerializable
{
    public int Strength { get; set; }
    public int Defense { get; set; }
    public int Agility { get; set; }
    public int Intelligence { get; set; }
    public int Willpower { get; set; }
    
    public void Deserialize(ICustomDataReader reader)
    {
        Strength = reader.GetInt();
        Defense = reader.GetInt();
        Agility = reader.GetInt();
        Intelligence = reader.GetInt();
        Willpower = reader.GetInt();
    }
    
    public void Serialize(ICustomDataWriter writer)
    {
        writer.Put(Strength);
        writer.Put(Defense);
        writer.Put(Agility);
        writer.Put(Intelligence);
        writer.Put(Willpower);
    }
}