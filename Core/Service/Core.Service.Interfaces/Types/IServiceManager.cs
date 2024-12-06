namespace Core.Service.Interfaces.Types;

public interface IServiceManager : ISingleService
{
    IServiceProvider ServiceProvider { get; }
}