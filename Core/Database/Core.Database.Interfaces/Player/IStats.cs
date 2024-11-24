namespace Core.Database.Interfaces.Player;

public interface IStats : IEntity
{
    int Strength { get; set; }
    int Defense { get; set; }
    int Agility { get; set; }
    int Intelligence { get; set; }
    int Willpower { get; set; }
    
    public string ToString()
    {
        return $"Strength: {Strength}, Defense: {Defense}, Agility: {Agility}, Intelligence: {Intelligence}, Willpower: {Willpower}";
    }
}