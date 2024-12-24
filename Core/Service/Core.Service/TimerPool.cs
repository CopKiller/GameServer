using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Service;

internal class TimerPool(IServiceConfiguration configuration, ILogger<TimerPool>? logger) : IDisposable
{
    private Stopwatch MainTimer { get; } = new();
    private ConcurrentDictionary<ISingleService, long> ServiceLastTick { get; } = new();
    private Task? UpdateLoopTask { get; set; }

    public void Start(CancellationToken cancellationToken)
    {
        if (UpdateLoopTask != null) return;

        MainTimer.Start();

        UpdateLoopTask = Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var startTick = MainTimer.ElapsedMilliseconds;
                
                // Usar foreach para atualizar todos os serviços em sequência.
                foreach (var service in ServiceLastTick.Keys)
                {
                    try
                    {
                        Update(service);
                    }
                    catch (Exception e)
                    {
                        logger?.LogError(e, "Erro ao atualizar o serviço {ServiceType}.",
                            service.ServiceConfiguration.ServiceType);
                    }
                }

                // Usar Parallel.ForEach para atualizar todos os serviços em paralelo.
                /*Parallel.ForEach(ServiceLastTick.Keys, service =>
                {
                    try
                    {
                        Update(service);
                    }
                    catch (Exception e)
                    {
                        logger?.LogError(e, "Erro ao atualizar o serviço {ServiceType}.",
                            service.ServiceConfiguration.ServiceType);
                    }
                });*/

                var elapsed = MainTimer.ElapsedMilliseconds - startTick;
                if (elapsed > configuration.UpdateIntervalMs)
                    logger?.LogWarning("Loop de atualização atrasado em {Elapsed}ms.", elapsed);
                else
                    await Task.Delay((int)(configuration.UpdateIntervalMs - elapsed), cancellationToken);
            }
        }, cancellationToken);
    }


    internal void Update(ISingleService service, bool force = false)
    {
        if (!service.ServiceConfiguration.Enabled) return;
        if (!service.ServiceConfiguration.NeedUpdate) return;

        ServiceLastTick.TryAdd(service, 0); // Tentativa de adicionar o serviço caso não exista

        var tick = MainTimer.ElapsedMilliseconds;

        var lastTick = ServiceLastTick.GetValueOrDefault(service, 0);

        // Valor inicial
        var tickCounter = tick - lastTick;
        if (tickCounter < service.ServiceConfiguration.UpdateIntervalMs && !force) return;

        service.Update(tick);
        ServiceLastTick[service] = tick;
    }

    public async Task StopAsync()
    {
        if (UpdateLoopTask != null)
            try
            {
                await UpdateLoopTask;
            }
            catch (TaskCanceledException)
            {
                // Ignorar cancelamento
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Erro ao aguardar o término da tarefa de atualização.");
            }
            finally
            {
                UpdateLoopTask.Dispose();
                UpdateLoopTask = null;
            }
    }


    public void AddService<T>(T service) where T : ISingleService
    {
        ServiceLastTick.TryAdd(service, 0);
    }

    public void Dispose()
    {
        StopAsync().GetAwaiter().GetResult();

        foreach (var service in ServiceLastTick.Keys)
        {
            service.Dispose();
            logger?.LogDebug("Serviço {ServiceType} liberado.", service.ServiceConfiguration.ServiceType);
        }

        ServiceLastTick.Clear();
        MainTimer.Stop();
    }
}