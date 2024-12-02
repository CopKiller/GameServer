using System.Collections.Immutable;
using System.Diagnostics;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Logging;

namespace Core.Service;

internal class TimerPool(IServiceConfiguration configuration, ILogger<TimerPool>? logger) : IDisposable
{
    private Stopwatch MainTimer { get; } = new();
    private Dictionary<ISingleService, long> ServiceLastTick { get; } = new();
    private Task? UpdateTask { get; set; }
    private CancellationTokenSource? UpdateCancellationTokenSource { get; set; }

    public void Start(CancellationToken cancellationToken)
    {
        MainTimer.Start();

        UpdateTask = Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var startTick = MainTimer.ElapsedMilliseconds;

                foreach (var service in ServiceLastTick.Keys)
                {
                    Update(service);
                }

                var elapsed = MainTimer.ElapsedMilliseconds - startTick;
                if (elapsed < configuration.UpdateIntervalMs)
                {
                    await Task.Delay((int)(configuration.UpdateIntervalMs - elapsed), cancellationToken);
                }
            }

            Stop();
        }, cancellationToken);
    }

    internal void Update(ISingleService service, bool force = false)
    {
        if (!service.Configuration.NeedUpdate) return;

        var tick = MainTimer.ElapsedMilliseconds;

        var tickCounter = tick - ServiceLastTick[service];
        if (tickCounter < service.Configuration.UpdateIntervalMs && !force) return;

        service.Update(tick);
        ServiceLastTick[service] = tick;
    }

    public void Stop()
    {
        foreach (var service in ServiceLastTick.Keys.Where(s => s.Configuration.Enabled).ToImmutableList())
        {
            service.Stop();
            logger?.LogDebug($"{nameof(service.Configuration.ServiceType)} stopped.");
        }

        MainTimer.Stop();
    }

    public void AddService<T>(T service) where T : ISingleService
    {
        ServiceLastTick.TryAdd(service, 0);
    }

    public void Dispose()
    {
        foreach (var service in ServiceLastTick.Keys)
        {
            service.Dispose();
            logger?.LogDebug($"{nameof(Service)} disposed.");
        }

        ServiceLastTick.Clear();

        MainTimer.Stop();
    }
}