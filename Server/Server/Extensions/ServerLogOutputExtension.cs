using Core.Logger.Interface;
using Microsoft.Extensions.DependencyInjection;
using Server.Logger;

namespace Server.Extensions;

public static class ServerLogOutputExtension
{
    public static void AddServerLogOutput(this IServiceCollection services)
    {
        services.AddSingleton<ILogOutput, LoggerOutput>();
    }
}