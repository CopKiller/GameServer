using System.ComponentModel.DataAnnotations.Schema;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Player;

namespace Core.Database.Models.Player;

public class Stats : IStats
{
    public int Id { get; set; }
    public int Strength { get; set; } = 1;
    public int Defense { get; set; } = 1;
    public int Agility { get; set; } = 1;
    public int Intelligence { get; set; } = 1;
    public int Willpower { get; set; } = 1;

    [ForeignKey("PlayerModelId")] public int PlayerModelId { get; set; }
    public PlayerModel PlayerModel { get; set; }

    public override string ToString()
    {
        return
            $"Strength: {Strength}, Defense: {Defense}, Agility: {Agility}, Intelligence: {Intelligence}, Willpower: {Willpower}";
    }
}