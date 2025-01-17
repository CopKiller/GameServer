using System;
using System.Linq;
using System.Reflection;
using Core.Network.Packets.Interface.Handler;
using Core.Network.Packets.Interface.Request;
using Core.Network.Packets.Interface.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network;

public class ClientRegisterHandler(
    IHandlerRegistry registry,
    IServiceProvider serviceProvider,
    ILogger<ClientRegisterHandler> logger)
{
    
    public void RegisterHandlers()
    {
        RegisterHandlerRequest();
        RegisterHandlerResponse();
    }
    
    private void RegisterHandlers(Type type, string registerMethodName)
    {
        var handlers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        t.GetInterfaces().Any(i =>
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == type))
            .ToList();

        foreach (var handlerType in handlers)
        {
            try
            {
                var interfaceType = handlerType.GetInterfaces()
                    .First(i => i.GetGenericTypeDefinition() == type);
                
                var requestType = interfaceType.GetGenericArguments()[0]; 

                // Resolver logger e criar a instância do handler
                var handlerInstance = ActivatorUtilities.CreateInstance(serviceProvider, handlerType);

                // Invocar o método de registro de forma genérica
                var method = typeof(IHandlerRegistry).GetMethod(registerMethodName)?
                    .MakeGenericMethod(requestType);

                if (method == null)
                {
                    logger.LogError($"Método '{registerMethodName}' não encontrado para {requestType.Name}");
                    continue;
                }

                method.Invoke(registry, new[] { handlerInstance });
                logger.LogInformation($"Handler registrado com sucesso: {requestType.Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao registrar handler: {handlerType.Name}. " +
                                    $"Detalhes: Constructor parameters may be incorrect or missing in the DI container. " +
                                    $"Interfaces encontradas: {string.Join(", ", handlerType.GetInterfaces().Select(i => i.Name))}");
            }

        }
    }

    /// <summary>
    /// Registrar todos os handlers de Request
    /// </summary>
    private void RegisterHandlerRequest()
    {
        RegisterHandlers(typeof(IHandlerRequest<>), nameof(IHandlerRegistry.RegisterRequestHandler));
    }

    /// <summary>
    /// Registrar todos os handlers de Response
    /// </summary>
    private void RegisterHandlerResponse()
    {
        RegisterHandlers(typeof(IHandlerResponse<>), nameof(IHandlerRegistry.RegisterResponseHandler));
    }
}