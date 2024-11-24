
namespace Core.Service.Interfaces.Types;

public interface ISingleService : ITransientService
{
    public IServiceConfiguration Configuration { get; }
    void Register();
    void Restart();
}