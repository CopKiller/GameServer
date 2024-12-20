using System.Numerics;
using Core.Physics.Interface;
using Core.Physics.Interface.Builder;
using Core.Physics.Interface.Enum;
using Core.Server.Extensions;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Tests.Utils;

public class PhysicsTest
{
    private IServiceProvider? _serviceProvider;
    
    private void Setup()
    {
        var services = new ServiceCollection();
        services.AddPhysics();
        services.AddServiceManager();
        services.AddLogger();
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    [Fact]
    public async Task GetBody()
    {
        Setup();
        
        if (_serviceProvider == null)
        {
            Assert.Null(_serviceProvider);
            return;
        }
        
        var bodyDef = _serviceProvider.GetRequiredService<IBodyBuilder>();
        
        bodyDef.Id = 1;
        bodyDef.EntityType = EEntityType.Player;
        bodyDef.Width = 32;
        bodyDef.Height = 32;
        bodyDef.Density = 1;
        //bodyDef.Angle = 0.0f;
        //bodyDef.Position = new Vector2(30, 30);
        bodyDef.Enabled = false;
        //bodyDef.FixedRotation = false;
        //bodyDef.LinearDamping = 0;
        //bodyDef.AngularDamping = 0;
        //bodyDef.GravityScale = 1;
        //bodyDef.LinearVelocity = new Vector2(0, 0);
        //bodyDef.AngularVelocity = 0;
        bodyDef.Awake = true;
        bodyDef.AllowSleep = true;
        bodyDef.Type = EBodyType.Dynamic;
        
        var worldBuilder = _serviceProvider.GetRequiredService<IWorldBuilder>();
        
        worldBuilder.Id = 1;
        
        var worldBuildedTuple = worldBuilder.Build();
        
        var worldBuilded = worldBuildedTuple.Item1;
        
        var worldEvents = worldBuildedTuple.Item2;
        // Adiciona eventos ao mundo
        worldEvents.OnBodyAdded += Assert.NotNull;
        
        worldEvents.OnBodyRemoved += Assert.NotNull;
        
        var worldManager = _serviceProvider.GetRequiredService<IWorldManager>();
        
        var resultWorldAdd = worldManager.AddWorld(worldBuilder.Id, worldBuilded);
        
        Assert.True(resultWorldAdd);
        
        var serviceManager = _serviceProvider.GetRequiredService<IServiceManager>();
        serviceManager.Register();
        serviceManager.Start();
        serviceManager.Update(0);
        
        await Task.Delay(100);
        
        var worldPhysics = worldManager.GetWorld(worldBuilder.Id);
        
        Assert.NotNull(worldPhysics);

        var bodyPhysics = worldPhysics.AddBody(bodyDef, true);
        
        Assert.NotNull(bodyPhysics);
        
        bodyPhysics.SetEnabled(true);
        
        var getBody = worldPhysics.GetBody(bodyDef.Id, EEntityType.Player);
        
        Assert.NotNull(getBody);
        
        var initialBodyPosition = getBody.GetPosition();
        
        await Task.Delay(100);
        
        var finalBodyPosition = getBody.GetPosition();
        
        Assert.NotEqual(initialBodyPosition, finalBodyPosition);
        
        getBody.ApplyForce(new Vector2(40000, 0));
        
        worldPhysics.RemoveBody(bodyDef.Id, EEntityType.Player);
        
        var getBodyRemoved = worldPhysics.GetBody(bodyDef.Id, EEntityType.Player);
        
        Assert.Null(getBodyRemoved);
        
    }
}