using Microsoft.Extensions.DependencyInjection;

namespace Core.Service.Interfaces.Types;

public interface IService : IDisposable
{
    void Start();
    void Stop();
}