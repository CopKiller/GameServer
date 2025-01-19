using System.Numerics;
using Core.Physics.Interface;
using Core.Physics.Interface.Builder;
using Core.Physics.Interface.Dynamics;
using Core.Physics.Interface.Dynamics.Events;
using Core.Physics.Dynamics;
using Core.Physics.Dynamics.Events;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Dynamics.Events;

namespace Core.Physics.Builder;

public class WorldBuilder : IWorldBuilder
{
    public int Id { get; set; }
    
    public Vector2 Gravity { get; set; } = new Vector2(0, 9.81f);
    
    public List<Vector2> Bounds { get; set; } = [];
    
    public void SetDefaults()
    {
        Bounds.Add(new Vector2(0, 0));
        Bounds.Add(new Vector2(0, 500));
        Bounds.Add(new Vector2(500, 500));
        Bounds.Add(new Vector2(500, 0));
    }

    public (IWorldPhysics, IWorldEvents) Build()
    {
        var velcroEvents = new WorldVelcroEvents();
        var worldEvents = new WorldEvents(velcroEvents);
        
        var worldPhysics = new WorldPhysics(new World(Gravity, velcroEvents));
        
        return (worldPhysics, worldEvents);
    }
    
    
}