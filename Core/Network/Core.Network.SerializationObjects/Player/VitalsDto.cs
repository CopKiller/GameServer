using Core.Network.Interface;

namespace Core.Network.SerializationObjects.Player;

public class VitalsDto : ICustomSerializable
{
    public double Health { get; set; }
    public double MaxHealth { get; set; }
    public double Mana { get; set; }
    public double MaxMana { get; set; }

    public void Deserialize(ICustomDataReader reader)
    {
        Health = reader.GetDouble();
        MaxHealth = reader.GetDouble();
        Mana = reader.GetDouble();
        MaxMana = reader.GetDouble();
    }

    public void Serialize(ICustomDataWriter writer)
    {
        writer.Put(Health);
        writer.Put(MaxHealth);
        writer.Put(Mana);
        writer.Put(MaxMana);
    }
}