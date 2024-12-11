using Core.Physics.Interface;
using Core.Physics.Tests;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;

namespace Core.Physics;

public class PhysicService(IWorldService worldService) : ISingleService
{
    public IServiceConfiguration ServiceConfiguration { get; } = new PhysicsConfiguration();
    private long _lastTick;
    public void Register() { }

    public void Start()
    {
        ServiceConfiguration.Enabled = true;
        worldService.Start();
    }
    
    public void Stop()
    {
        ServiceConfiguration.Enabled = false;
        worldService.Stop();
    }

    public void Dispose()
    {
        Stop();
        worldService.Dispose();
    }

    public void Update(long currentTick)
    {
        var deltaTime = (currentTick - _lastTick) / 1000f;
        worldService.Update(deltaTime);
        _lastTick = currentTick;
    }
    public void Restart()
    {
        worldService.Stop();
        worldService.Start();
    }
}