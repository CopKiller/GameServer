using Core.Physics.Abstraction.Enum;

namespace Core.Physics.Abstraction;

public interface IBodyData
{
    public int Id { get; }
    
    public EEntityType Type { get; }
    
    public IWorldPhysics World { get; }
    
    public IBodyPhysics BodyPhysics { get; }
}