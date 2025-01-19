using Core.Physics.Interface;
using Core.Physics.Interface.Dynamics;

namespace Core.Physics;

public class WorldService : IWorldService
{
    private readonly Dictionary<int, IWorldPhysics> _worlds = [];
    
    public void AddWorld(int id, IWorldPhysics world) => _worlds.Add(id, world);
    
    public void RemoveWorld(int id) => _worlds.Remove(id);
    
    public bool TryGetWorld(int id, out IWorldPhysics? world) => _worlds.TryGetValue(id, out world);
    
    public bool TryGetAllWorld(out IEnumerable<IWorldPhysics> world)
    {
        world = _worlds.Values.ToList();
        return world.Any();
    }
    
    public void Update(float deltaTime)
    {
        foreach (var world in _worlds.Values)
        {
            world.Update(deltaTime);
        }
    }
    
    public void Start()
    {
        foreach (var world in _worlds.Values)
        {
            world.Start();
        }
    }
    
    public void Stop()
    {
        foreach (var world in _worlds.Values)
        {
            world.Stop();
        }
    }
    
    public void Dispose()
    {
        foreach (var world in _worlds.Values)
        {
            world.Dispose();
        }
    }
    
}