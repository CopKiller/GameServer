using Core.Logger.Interface;
using Game.Scripts.Logger;
using Game.Scripts.Singletons;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Scripts.Extensions;

public static class GodotServicesExtension
{
    public static void AddGodotLoggerOutput(this IServiceCollection services)
    {
        services.AddSingleton<ILogOutput, GodotLogOutput>();
    }
}