namespace Core.Database.Interface.Player;

public interface IPosition
{
    float X { get; set; }
    float Y { get; set; }
    int Z { get; set; }
    double Rotation { get; set; }

    public string ToString()
    {
        return $"X: {X}, Y: {Y}, Z: {Z}, Rotation: {Rotation}";
    }
}