using System.ComponentModel.DataAnnotations;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Database.Interfaces.Player;
using Core.Database.Models.Player;

namespace Core.Database.Models.Account;

public class AccountModel : IAccountModel
{
    public int Id { get; set; }
    
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; }
    
    [Required]
    public required string Email { get; set; }
    
    [Required]
    public required string BirthDate { get; set; }
    
    public List<PlayerModel> Players { get; set; } = [];
    IEnumerable<IPlayerModel> IAccountModel.Players
    {
        get => Players;
        set => Players = value.Cast<PlayerModel>().ToList();
    }
    
}