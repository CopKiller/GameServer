using Core.Physics.Interface;
using Core.Physics.Dynamics;
using Core.Service.Interfaces;

namespace Core.Physics.Tests;

public class PhysicsConfiguration : IServiceConfiguration
{
    public Type ServiceType { get; } = typeof (WorldPhysics);
    public bool Enabled { get; set; } = false;
    public bool NeedUpdate { get; set; } = true;
    public int UpdateIntervalMs { get; set; } = 1;
}