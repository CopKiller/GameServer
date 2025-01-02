namespace Core.Service.Interfaces.Types;

public interface IServiceManager : IService
{
    IServiceProvider ServiceProvider { get; }
}