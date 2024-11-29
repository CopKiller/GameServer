namespace Core.Network.Models;

public class AccountDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<PlayerDTO> Players { get; set; } = new();
}