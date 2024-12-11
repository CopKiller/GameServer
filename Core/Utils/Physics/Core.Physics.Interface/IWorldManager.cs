using System.Numerics;
using Core.Physics.Interface.Dynamics;

namespace Core.Physics.Interface;

public interface IWorldManager
{
    bool AddWorld(int id, IWorldPhysics world);
    void RemoveWorld(int id);
    IEnumerable<IWorldPhysics> GetWorlds();
    IWorldPhysics? GetWorld(int id);
}