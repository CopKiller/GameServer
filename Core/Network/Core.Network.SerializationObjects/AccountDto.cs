using Core.Network.Interface;

namespace Core.Network.SerializationObjects;

public class AccountDto : IAdapterSerializable
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string BirthDate { get; set; } = string.Empty;
    
    public List<PlayerDto> Players { get; set; } = [];

    public void Serialize(IAdapterDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(Username);
        writer.Put(Email);
        writer.Put(Password);
        writer.Put(BirthDate);

        // Serializando a lista de Players
        writer.Put(Players.Count);  // Grava o número de jogadores na lista
        foreach (var player in Players)
        {
            player.Serialize(writer);  // Serializa cada PlayerDto
        }
    }

    public void Deserialize(IAdapterDataReader reader)
    {
        Id = reader.GetInt();
        Username = reader.GetString();
        Email = reader.GetString();
        Password = reader.GetString();
        BirthDate = reader.GetString();

        // Desserializando a lista de Players
        int playersCount = reader.GetInt();  // Lê o número de jogadores
        Players = new List<PlayerDto>(playersCount);  // Inicializa a lista de jogadores

        for (int i = 0; i < playersCount; i++)
        {
            var player = new PlayerDto();
            player.Deserialize(reader);  // Desserializa cada PlayerDto
            Players.Add(player);  // Adiciona à lista de Players
        }
    }
}