using Core.Network.Interface;

namespace Core.Network.SerializationObjects;

public class AccountDto : ICustomSerializable
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<PlayerDto?> Players { get; set; } = new();

    public void Serialize(ICustomDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(Username);
        writer.Put(Email);
    }

    public void Deserialize(ICustomDataReader reader)
    {
        Id = reader.GetInt();
        Username = reader.GetString();
        Email = reader.GetString();
    }
}