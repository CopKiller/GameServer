using System.Numerics;

namespace Core.Physics.Def;

public class WorldDefBuilder(int id, Vector2 gravity)
{
    public int Id { get; } = id;

    public Vector2 Gravity { get; } = gravity;

    public List<Vector2> Bounds { get; } = [];
}