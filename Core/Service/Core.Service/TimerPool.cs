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
    
    private readonly ManualResetEventSlim _loopWaitHandle = new();

    public void Start(CancellationToken cancellationToken)
{
    if (UpdateLoopTask != null) return;

    configuration.Enabled = true;

    MainTimer.Start();

    UpdateLoopTask = Task.Run(() =>
    {
        while (!cancellationToken.IsCancellationRequested && configuration.Enabled)
        {
            var startTick = MainTimer.ElapsedMilliseconds;

            foreach (var service in ServiceLastTick.Keys)
            {
                try
                {
                    Update(service);
                }
                catch (Exception e)
                {
                    logger?.LogError(e, "Erro ao atualizar o serviço {ServiceType}.", service.ServiceConfiguration.ServiceType);
                }
            }

            var elapsed = MainTimer.ElapsedMilliseconds - startTick;
            var delay = Math.Max(0, configuration.UpdateIntervalMs - elapsed);

            if (elapsed > configuration.UpdateIntervalMs)
                logger?.LogWarning("Loop de atualização atrasado em {Elapsed}ms.", elapsed);

            _loopWaitHandle.Wait((int)delay, cancellationToken);
        }
    }, cancellationToken);
}


    internal void Update(ISingleService service, bool force = false)
    {
        if (!configuration.UpdateWithManager)
        {
            logger?.LogDebug("Atualização de serviço desabilitada.");
            return;
        }
        if (!service.ServiceConfiguration.Enabled) return;
        if (!service.ServiceConfiguration.UpdateWithManager) return;

        ServiceLastTick.TryAdd(service, 0);

        var tick = MainTimer.ElapsedMilliseconds;

        var lastTick = ServiceLastTick.GetValueOrDefault(service, 0);
        
        var tickCounter = tick - lastTick;
        if (tickCounter < service.ServiceConfiguration.UpdateIntervalMs && !force) return;

        service.Update(tick);
        ServiceLastTick.AddOrUpdate(service, tick, (_, _) => tick);
    }

    public async Task StopAsync()
    {
        configuration.Enabled = false;
        
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

    public async void Dispose()
    {
        await StopAsync();

        foreach (var service in ServiceLastTick.Keys)
        {
            service.Dispose();
            logger?.LogDebug("Serviço {ServiceType} liberado.", service.ServiceConfiguration.ServiceType);
        }

        ServiceLastTick.Clear();
        MainTimer.Stop();
    }
}