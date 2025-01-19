using Microsoft.Extensions.DependencyInjection;

namespace Core.Service.Interfaces.Types;

public interface IService : IDisposable
{
    void Register();
    void Start();
    void Stop();
    void Restart();
    void Update(long currentTick);
}