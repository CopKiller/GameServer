using Core.Database.Interface.Player;

namespace Core.Database.Interface.Account;

public interface IAccountModel : IEntity
{
    string Username { get; set; }
    string Password { get; set; }
    string Email { get; set; }
    DateOnly BirthDate { get; set; }
    DateTime CreatedAt { get; set; }
    ICollection<IPlayerModel> Players { get; set; }
}