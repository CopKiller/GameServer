using System.Numerics;
using Core.Physics.Interface;
using Core.Physics.Interface.Builder;
using Core.Physics.Interface.Dynamics;
using Core.Physics.Dynamics;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Dynamics.Events;

namespace Core.Physics.Shared;

// PhysicsManager: Centraliza a criação e gerenciamento de mundos
public class WorldManager(IWorldService service) : IWorldManager
{
    public bool AddWorld(int id, IWorldPhysics world)
    {
        if (service.TryGetWorld(id, out _))
        {
            return false;
        }
        
        service.AddWorld(id, world);
        return true;
    }
    
    public void RemoveWorld(int id)
    {
        service.RemoveWorld(id);
    }

    public IEnumerable<IWorldPhysics> GetWorlds() => service.TryGetAllWorld(out var worlds) ? worlds : Array.Empty<IWorldPhysics>();

    public IWorldPhysics? GetWorld(int id) => service.TryGetWorld(id, out var world) ? world : null;
}