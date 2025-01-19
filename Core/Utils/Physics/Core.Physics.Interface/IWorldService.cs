using Core.Physics.Interface.Dynamics;

namespace Core.Physics.Interface;

public interface IWorldService
{
    void AddWorld(int id, IWorldPhysics world);
    void RemoveWorld(int id);
    bool TryGetWorld(int id, out IWorldPhysics? world);
    bool TryGetAllWorld(out IEnumerable<IWorldPhysics> world);
    void Start();
    void Update(float deltaTime);
    void Stop();
    void Dispose();
}