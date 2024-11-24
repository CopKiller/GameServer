namespace Core.Service.Interfaces.Types;

public interface ITransientService : IService
{
    void Update(long currentTick);
}