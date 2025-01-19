using System.Numerics;
using Core.Physics.Interface.Dynamics;
using Core.Physics.Interface.Dynamics.Events;

namespace Core.Physics.Interface.Builder;

public interface IWorldBuilder
{
    int Id { get; set; }
    Vector2 Gravity { get; set; }
    List<Vector2> Bounds { get; set; }
    void SetDefaults();
    
    (IWorldPhysics, IWorldEvents) Build();
}