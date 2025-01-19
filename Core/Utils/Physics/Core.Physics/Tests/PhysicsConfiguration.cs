using Core.Physics.Interface;
using Core.Physics.Dynamics;
using Core.Service.Interfaces;

namespace Core.Physics.Tests;

public class PhysicsConfiguration : IServiceConfiguration
{
    public Type ServiceType { get; } = typeof (WorldPhysics);
    public bool Enabled { get; set; } = false;
    public bool StartWithManager { get; set; } = true;
    public bool StopWithManager { get; set; } = true;
    public bool UpdateWithManager { get; set; } = true;
    public int UpdateIntervalMs { get; set; } = 5;
}