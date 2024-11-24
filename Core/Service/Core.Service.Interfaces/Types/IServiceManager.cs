namespace Core.Service.Interfaces.Types;

public interface IServiceManager : IService
{
    public IServiceConfiguration Configuration { get; }
    public IServiceProvider? ServiceProvider { get; set; }
    public void Register();
}