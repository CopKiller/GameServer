using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database.Interface;
using Core.Database.Interface.Player;
using Core.Database.Models.Account;

namespace Core.Database.Models.Player;

public class PlayerModel : IPlayerModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public byte SlotNumber { get; set; }
    
    public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    
    public DateOnly LastLogin { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public int Level { get; set; }

    public int Experience { get; set; }

    public int Golds { get; set; }
    
    public int Diamonds { get; set; }

    public Vitals Vitals { get; set; } = new();

    IVitals IPlayerModel.Vitals
    {
        get => Vitals;
        set => Vitals = (Vitals)value;
    }

    public Stats Stats { get; set; } = new();

    IStats IPlayerModel.Stats
    {
        get => Stats;
        set => Stats = (Stats)value;
    }

    public Position Position { get; set; } = new();

    IPosition IPlayerModel.Position
    {
        get => Position;
        set => Position = (Position)value;
    }
    
    public int AccountModelId { get; set; }
    public AccountModel AccountModel { get; set; } = null!;
}