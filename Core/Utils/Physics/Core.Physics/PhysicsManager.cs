using System.Numerics;
using Core.Physics.Abstraction;

namespace Core.Physics;

// PhysicsManager: Centraliza a criação e gerenciamento de mundos
public class PhysicsManager : IPhysicsManager
{
    private readonly List<IWorldPhysics> _worlds = new();

    public IWorldPhysics CreateWorld(int id, Vector2 gravity)
    {
        var world = new WorldPhysics(id, gravity);
        _worlds.Add(world);
        return world;
    }

    public void RemoveWorld(IWorldPhysics world)
    {
        _worlds.Remove(world);
    }

    public IEnumerable<IWorldPhysics> GetWorlds() => _worlds;

    public IWorldPhysics? GetWorld(int id) => _worlds.FirstOrDefault(w => w.Id == id);
}