using System.Numerics;

namespace Core.Physics.Abstraction;

public interface IPhysicsManager
{
    IWorldPhysics CreateWorld(int id, Vector2 gravity);
    void RemoveWorld(IWorldPhysics world);
    IEnumerable<IWorldPhysics> GetWorlds();
    IWorldPhysics? GetWorld(int id);
}