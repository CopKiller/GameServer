
using System.Numerics;
using Core.Physics.Abstraction;
using Core.Physics.Abstraction.Enum;
using Core.Physics.Def;
using Core.Physics.Prefab;
using Core.Physics.Tests;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Definitions;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Utilities;
using Microsoft.Xna.Framework;

namespace Core.Physics;

// Representa um mundo físico
public sealed class WorldPhysics : ISingleService, IWorldPhysics
{
    private readonly World _world;
    private readonly Dictionary<IBodyData, Body> _bodies = new();

    public int Id { get; }
    public Vector2 Gravity { get; }

    public WorldPhysics(int id, Vector2 gravity)
    {
        Id = id;
        Gravity = gravity;
        _world = new World(new Vector2(gravity.X, gravity.Y));
    }

    public IBodyData AddBody(IBodyDefBuilder iBodyDef)
    {
        var velcroBody = BodyFactory.CreateFromDef(_world, iBodyDef.ToVelcroPhysics());

        switch (iBodyDef.EntityType)
        {
            case EEntityType.Object:
                var circleShape = new CircleShape(iBodyDef.Width / 2, iBodyDef.Density);
                velcroBody.AddFixture(circleShape);
                break;
            case EEntityType.Player: case EEntityType.Npc:
                var rectangleVertices = PolygonUtils.CreateRectangle(iBodyDef.Width / 2, iBodyDef.Height / 2);
                //rectangleVertices.Translate(ref iBodyDef.Offset);
                var polygonShape = new PolygonShape(rectangleVertices, iBodyDef.Density);
                velcroBody.AddFixture(polygonShape);
                break;
        }
        
        var bodyPhysics = new BodyPhysics(velcroBody);
        var bodyData = new BodyData(iBodyDef.Id, iBodyDef.EntityType, this, bodyPhysics);
        _bodies.Add(bodyData, velcroBody);
        return bodyData;
    }
    
    public void AddFixture()
    {
        BodyFactory.CreateRectangle(_world, 500, 500, 1, new Vector2(0, 0));
    }

    public void RemoveBody(IBodyData body)
    {
        if (!_bodies.Remove(body, out var velcroBody))
            return;

        _world.RemoveBody(velcroBody);
    }

    public IEnumerable<IBodyData> GetBodies() => _bodies.Keys;

    public IBodyData? GetBody(int id) => _bodies.Keys.FirstOrDefault(b => b.Id == id);

    public void Start() => _world.Enabled = true;

    public void Update(float deltaTime) => _world.Step(deltaTime);

    public void Stop() => _world.Enabled = false;

    public void Clear()
    {
        Stop();
        _world.Clear();
    }

    public void Dispose()
    {
        Clear();
        _bodies.Clear();
    }

    public IServiceConfiguration ServiceConfiguration { get; } = new PhysicsConfiguration();
    public void Register()
    {
        throw new NotImplementedException();
    }

    public void Update(long currentTick)
    {
        throw new NotImplementedException();
    }

    public void Restart()
    {
        throw new NotImplementedException();
    }
}