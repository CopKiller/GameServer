using Core.Physics.Abstraction;
using Core.Physics.Abstraction.Enum;
using Genbox.VelcroPhysics.Dynamics;

namespace Core.Physics.Prefab;

// Representa os dados de um corpo físico
public class BodyData(int id, EEntityType type, IWorldPhysics world, IBodyPhysics body)
    : IBodyData
{
    public int Id { get; } = id;
    public EEntityType Type { get; } = type;
    public IWorldPhysics World { get; } = world;
    public IBodyPhysics BodyPhysics { get; } = body;
}