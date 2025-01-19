using System.ComponentModel.DataAnnotations.Schema;
using Core.Database.Interface;
using Core.Database.Interface.Player;

namespace Core.Database.Models.Player;

public class Vitals : IVitals
{
    public int Id { get; set; }
    public double Health { get; set; }
    public double MaxHealth { get; set; }
    public double Mana { get; set; }
    public double MaxMana { get; set; }

    [ForeignKey("PlayerModelId")] public int PlayerModelId { get; set; }
    public PlayerModel PlayerModel { get; set; } = null!;

    public override string ToString()
    {
        return $"Health: {Health}/{MaxHealth}, Mana: {Mana}/{MaxMana}";
    }
}