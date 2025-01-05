using Core.Network.Interface;

namespace Core.Network.SerializationObjects;

public class AccountDto : IAdapterSerializable
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<PlayerDto?> Players { get; set; } = new();

    public void Serialize(IAdapterDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(Username);
        writer.Put(Email);
    }

    public void Deserialize(IAdapterDataReader reader)
    {
        Id = reader.GetInt();
        Username = reader.GetString();
        Email = reader.GetString();
    }
}