namespace Core.Network.DtoObjects;

public class AccountDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<PlayerDto> Players { get; set; } = new();
}