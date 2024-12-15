using System.ComponentModel.DataAnnotations;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Database.Interfaces.Player;
using Core.Database.Models.Player;

namespace Core.Database.Models.Account;

public class AccountModel : IAccountModel
{
    public int Id { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public DateOnly BirthDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public ICollection<PlayerModel> Players { get; set; } = new List<PlayerModel>();
    
    ICollection<IPlayerModel> IAccountModel.Players
    {
        get => Players.Cast<IPlayerModel>().ToList();
        set => Players = value?.OfType<PlayerModel>().ToList() ?? [];
    }
}
