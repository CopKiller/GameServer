using System.ComponentModel.DataAnnotations.Schema;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Player;

namespace Core.Database.Models.Player;

public class Position : IEntity, IPosition
{
    public int Id { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public int Z { get; set; }
    public double Rotation { get; set; }

    [ForeignKey("PlayerModelId")] public int PlayerModelId { get; set; }
    public PlayerModel PlayerModel { get; set; }

    public override string ToString()
    {
        return $"X: {X}, Y: {Y}, Z: {Z}, Rotation: {Rotation}";
    }
}