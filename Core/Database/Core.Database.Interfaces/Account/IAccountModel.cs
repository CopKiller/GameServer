using Core.Database.Interfaces.Player;

namespace Core.Database.Interfaces.Account;

public interface IAccountModel : IEntity
{
    string Username { get; set; }
    string Password { get; set; }
    string Email { get; set; }
    string BirthDate { get; set; }
    IEnumerable<IPlayerModel> Players { get; set; }
}