namespace Core.Physics.Abstraction;

public interface IWorldPhysics
{
    int Id { get; }
    IBodyData AddBody(IBodyDefBuilder iBodyDef);
    void RemoveBody(IBodyData body);
    IEnumerable<IBodyData> GetBodies();
    IBodyData? GetBody(int id);
    void Start();
    void Update(float deltaTime);
    void Stop();
    void Dispose();
    void Clear();
}