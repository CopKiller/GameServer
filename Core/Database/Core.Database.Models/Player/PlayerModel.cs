using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Player;
using Core.Database.Models.Account;

namespace Core.Database.Models.Player;

public class PlayerModel : IPlayerModel
{
    public int Id { get; set; }

    [Required]
    public int SlotNumber { get; set; }

    [Required]
    public required string Name { get; set; }
    
    [Required]
    public int Level { get; set; }
    
    [Required]
    public int Experience { get; set; }
    
    [Required]
    public int Gold { get; set; }

    [Required]
    public required Vitals Vitals { get; set; }
    IVitals IPlayerModel.Vitals { get => Vitals; set => Vitals = (Vitals)value; }
    
    [Required]
    public required Stats Stats { get; set; }
    IStats IPlayerModel.Stats { get => Stats; set => Stats = (Stats)value; }
    
    [Required]
    public required Position Position { get; set; }
    IPosition IPlayerModel.Position { get => Position; set => Position = (Position)value; }


    [ForeignKey("AccountModelId")] // Nome da propriedade de navegação
    public int AccountModelId { get; set; }
    
    public AccountModel AccountModel { get; set; }

    public override string ToString()
    {
        return $"PlayerModel: {Id}, {SlotNumber}, {Name}, {Level}, {Experience}, {Gold}, {Vitals}, {Stats}, {Position}, {AccountModelId}";
    }
}