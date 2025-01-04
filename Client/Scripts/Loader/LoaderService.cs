using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Microsoft.Extensions.Logging;
using Array = Godot.Collections.Array;

namespace Game.Scripts.Loader;

public class LoaderService(ILogger<LoaderService> logger) : ILoaderService
{
    private const bool UseLoadThread = false;
    private const bool UseSubThreads = false;
    private const bool SendProgressSignals = false;
    private const int MaxConcurrentLoads = 4;
    private const float LoadTimeout = 20.0f;
    private const float LoadProgressUpdateInterval = 0.1f;
    private const float LoadProgressUpdateThreshold = 0.01f;

    public event Action<string>? LoadStarted;
    public event Action<string, float>? LoadProgress;
    public event Action<string, Resource>? LoadCompleted;
    public event Action<string, string>? LoadFailed;

    private readonly PriorityQueue<string, LoaderPriority> _loadQueue = new();
    private readonly HashSet<string> _activeLoads = new();
    private readonly Dictionary<string, Resource> _loadedResources = new();
    private readonly Array _progress = [];
    private readonly Dictionary<string, float> _loadTimers = new();
    private int _concurrentLoads = 0;

    public void EnqueueResource(string resourcePath, LoaderPriority priority)
    {
        if (_loadedResources.ContainsKey(resourcePath) || _activeLoads.Contains(resourcePath))
        {
            logger.LogError($"Resource '{resourcePath}' is already loaded or in progress.");
            return;
        }

        _loadQueue.Enqueue(resourcePath, priority);
    }

    public void ProcessQueue()
    {
        while (_loadQueue.Count > 0 && _concurrentLoads < MaxConcurrentLoads)
        {
            StartNextLoad();
        }

        CheckLoadingStatus();
    }

    private void StartNextLoad()
    {
        if (_loadQueue.Count == 0 || _concurrentLoads >= MaxConcurrentLoads)
            return;

        var resourceToLoad = _loadQueue.Dequeue();
        _activeLoads.Add(resourceToLoad);
        _loadTimers[resourceToLoad] = 0f;
        _concurrentLoads++;

        try
        {
            Error result = Error.Ok;
            
            if (UseLoadThread)
            {
                result = ResourceLoader.LoadThreadedRequest(path: resourceToLoad, useSubThreads: UseSubThreads);
                
                if (result == Error.Ok)
                {
                    LoadStarted?.Invoke(resourceToLoad);
                }
                else
                {
                    logger.LogError($"Failed to request resource '{resourceToLoad}': {result}");
                    _activeLoads.Remove(resourceToLoad);
                    _concurrentLoads--;
                    LoadFailed?.Invoke(resourceToLoad, "Failed to start loading resource.");
                }
            }
            else
            {
                var resource = ResourceLoader.Load(path: resourceToLoad);
                
                if (resource != null)
                {
                    _loadedResources[resourceToLoad] = resource;
                    LoadCompleted?.Invoke(resourceToLoad, resource);
                }
                else
                {
                    LoadFailed?.Invoke(resourceToLoad, "Failed to load resource.");
                }
                
                _activeLoads.Remove(resourceToLoad);
                _concurrentLoads--;
                _loadTimers.Remove(resourceToLoad);
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception while requesting resource '{resourceToLoad}': {ex.Message}");
            _activeLoads.Remove(resourceToLoad);
            _concurrentLoads--;
            _loadTimers.Remove(resourceToLoad);
            LoadFailed?.Invoke(resourceToLoad, "Exception during resource load start.");
        }
    }

    private void CheckLoadingStatus()
    {
        foreach (var resourcePath in _activeLoads.ToList())
        {
            try
            {
                _loadTimers[resourcePath] += LoadProgressUpdateInterval;

                if (_loadTimers[resourcePath] >= LoadTimeout)
                {
                    logger.LogError($"Timeout loading resource '{resourcePath}'.");
                    _activeLoads.Remove(resourcePath);
                    _concurrentLoads--;
                    LoadFailed?.Invoke(resourcePath, "Load timeout exceeded.");
                    _loadTimers.Remove(resourcePath);
                    continue;
                }

                var status = ResourceLoader.LoadThreadedGetStatus(resourcePath, _progress);

                if (status == ResourceLoader.ThreadLoadStatus.InProgress)
                {
                    if (SendProgressSignals)
                    {
                        float progressValue = (float)_progress[0];
                        if (progressValue >= LoadProgressUpdateThreshold)
                        {
                            LoadProgress?.Invoke(resourcePath, progressValue);
                        }
                    }
                }
                else if (status == ResourceLoader.ThreadLoadStatus.Loaded)
                {
                    var resource = ResourceLoader.LoadThreadedGet(resourcePath);
                    if (resource != null)
                    {
                        _loadedResources[resourcePath] = resource;
                        LoadCompleted?.Invoke(resourcePath, resource);
                    }
                    else
                    {
                        LoadFailed?.Invoke(resourcePath, "Failed to retrieve loaded resource.");
                    }
                    _activeLoads.Remove(resourcePath);
                    _concurrentLoads--;
                    _loadTimers.Remove(resourcePath);
                }
                else
                {
                    LoadFailed?.Invoke(resourcePath, $"Loading failed with status: {status}");
                    _activeLoads.Remove(resourcePath);
                    _concurrentLoads--;
                    _loadTimers.Remove(resourcePath);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception during loading status check for '{resourcePath}': {ex.Message}");
                LoadFailed?.Invoke(resourcePath, "Exception during loading status check.");
                _activeLoads.Remove(resourcePath);
                _concurrentLoads--;
                _loadTimers.Remove(resourcePath);
            }
        }
    }
}
