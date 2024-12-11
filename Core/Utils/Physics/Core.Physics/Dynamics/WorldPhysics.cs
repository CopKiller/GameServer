using System.Numerics;
using Core.Physics.Interface;
using Core.Physics.Interface.Builder;
using Core.Physics.Interface.Dynamics;
using Core.Physics.Interface.Enum;
using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Definitions;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Utilities;

namespace Core.Physics.Dynamics;

public sealed class WorldPhysics(World world) : IWorldPhysics
{
    private readonly BodyTrackerAllocator _bodyTrackerAllocator = new();
    
    private bool AddBody(int id, EEntityType type, IBodyPhysics bodyPhysics)
    {
        var bodyTracker = _bodyTrackerAllocator.GetBodyTracker(type);
        
        if (bodyTracker.TryGetBody(id, out _))
        {
            return false;
        }
        
        bodyTracker.AddBody(id, bodyPhysics);
        return true;
    }
    
    public IBodyPhysics? AddBody(IBodyBuilder bodyBuilder, bool delayUntilNextStep = false)
    {
        var bodyDef = bodyBuilder.ToVelcroPhysics() as BodyDef;
        
        var velcroBody = BodyFactory.CreateFromDef(world, bodyDef, delayUntilNextStep);
        
        var bodyPhysics = new BodyPhysics(velcroBody);
        
        var result = AddBody(bodyBuilder.Id, bodyBuilder.EntityType, bodyPhysics);
        
        if (!result)
        {
            return null;
        }

        switch (bodyBuilder.EntityType)
        {
            case EEntityType.Object:
                var circleShape = new CircleShape(bodyBuilder.Width / 2, bodyBuilder.Density);
                velcroBody.AddFixture(circleShape);
                break;
            case EEntityType.Player: case EEntityType.Npc:
                var rectangleVertices = PolygonUtils.CreateRectangle(bodyBuilder.Width / 2, bodyBuilder.Height / 2);
                //rectangleVertices.Translate(ref iBodyDef.Offset);
                var polygonShape = new PolygonShape(rectangleVertices, bodyBuilder.Density);
                velcroBody.AddFixture(polygonShape);
                break;
        }
        
        return bodyPhysics;
    }
    
    public void AddFixture()
    {
        BodyFactory.CreateRectangle(world, 500, 500, 1, new Vector2(0, 0));
    }

    public bool RemoveBody(int id, EEntityType type)
    {
        var bodyTracker = _bodyTrackerAllocator.GetBodyTracker(type);
        
        if (!bodyTracker.TryGetBody(id, out var body))
        {
            return false;
        }
        
        body?.Dispose();
        
        return bodyTracker.RemoveBody(id);
    }

    public IEnumerable<IBodyPhysics> GetBodies(EEntityType type) => _bodyTrackerAllocator.GetBodyTracker(type).TryGetAllBodies(out var bodies) ? bodies : new List<IBodyPhysics>();

    public IBodyPhysics? GetBody(int id, EEntityType type) => _bodyTrackerAllocator.GetBodyTracker(type).TryGetBody(id, out var body) ? body : null;
    
    public void Start()
    {
        world.Enabled = true;
    }

    public void Stop()
    {
        world.Enabled = false;
    }
    
    public void Update(float deltaTime) => world.Step(deltaTime);
    
    public void Dispose()
    {
        Stop();
        
        world.Clear();
        
        _bodyTrackerAllocator.Clear();
    }
}