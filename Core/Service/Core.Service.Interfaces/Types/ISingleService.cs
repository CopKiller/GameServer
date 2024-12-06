namespace Core.Service.Interfaces.Types;

public interface ISingleService : IService
{
    public IServiceConfiguration ServiceConfiguration { get; }
    void Register();
    void Update(long currentTick);
    void Restart();
}