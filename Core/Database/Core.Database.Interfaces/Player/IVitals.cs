namespace Core.Database.Interfaces.Player;

public interface IVitals : IEntity
{
    double Health { get; set; }
    double MaxHealth { get; set; }
    double Mana { get; set; }
    double MaxMana { get; set; }
    
    public string ToString()
    {
        return $"Health: {Health}, MaxHealth: {MaxHealth}, Mana: {Mana}, MaxMana: {MaxMana}";
    }
}