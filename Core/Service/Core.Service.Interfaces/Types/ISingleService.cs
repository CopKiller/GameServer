
namespace Core.Service.Interfaces.Types;

public interface ISingleService : ITransientService
{
    public IServiceConfiguration ServiceConfiguration { get; }
    void Register();
    void Restart();
}