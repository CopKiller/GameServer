using Core.Physics.Interface;
using Core.Physics.Interface.Dynamics;
using Core.Physics.Interface.Enum;

namespace Core.Physics;

public class BodyTrackerAllocator
{
    private readonly Dictionary<EEntityType, BodyTracker<IBodyPhysics>> _bodies = new();
    
    public BodyTrackerAllocator()
    {
        foreach (var type in Enum.GetValues<EEntityType>())
        {
            _bodies.Add(type, new BodyTracker<IBodyPhysics>());
        }
    }
    
    public BodyTracker<IBodyPhysics> GetBodyTracker(EEntityType type)
    {
        return _bodies[type];
    }
    
    public void Clear()
    {
        foreach (var body in _bodies.Values)
        {
            body.Clear();
        }
    }
}

public class BodyTracker<T>
{
    private readonly Dictionary<int, T> _bodies = new();

    public void AddBody(int id, T body)
    {
        _bodies.Add(id, body);
    }

    public bool RemoveBody(int id)
    {
        return _bodies.Remove(id);
    }

    public bool TryGetBody(int id, out T? body)
    {
        return _bodies.TryGetValue(id, out body);
    }

    public bool TryGetAllBodies(out IEnumerable<T> bodies)
    {
        bodies = _bodies.Values.ToList();
        return bodies.Any();
    }
    
    public void Clear()
    {
        _bodies.Clear();
    }
}