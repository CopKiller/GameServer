using Core.Physics.Interface.Builder;
using Core.Physics.Interface.Enum;

namespace Core.Physics.Interface.Dynamics;

public interface IWorldPhysics
{
    IBodyPhysics? AddBody(IBodyBuilder bodyBuilder, bool delayUntilNextStep = false);
    bool RemoveBody(int id, EEntityType type);
    IEnumerable<IBodyPhysics> GetBodies(EEntityType type);
    IBodyPhysics? GetBody(int id, EEntityType type);
    
    void AddFixture();
    
    void Start();
    
    void Stop();
    
    void Dispose();
    
    void Update(float deltaTime);
}