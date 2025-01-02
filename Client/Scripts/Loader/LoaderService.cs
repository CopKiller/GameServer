using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using Array = Godot.Collections.Array;

namespace Game.Scripts.Loader;

public class LoaderService : ILoaderService
{
    public event Action<string>? LoadStarted;
    public event Action<string, float>? LoadProgress;
    public event Action<string, Resource?>? LoadCompleted;
    public event Action<string, string>? LoadFailed;

    private readonly PriorityQueue<string, LoaderPriority> _loadQueue = new();
    private string? _currentLoadingResource;
    private readonly Dictionary<string, Resource> _loadedResources = new();
    private readonly Array _progress = [];

    public void EnqueueResource(string resourcePath, LoaderPriority priority)
    {
        if (_loadedResources.ContainsKey(resourcePath) || resourcePath == _currentLoadingResource)
        {
            GD.Print($"Resource '{resourcePath}' is already loaded or in progress.");
            return;
        }

        _loadQueue.Enqueue(resourcePath, priority);
    }

    public void ProcessQueue()
    {
        if (_currentLoadingResource != null)
        {
            CheckLoadingStatus();
            return;
        }

        if (_loadQueue.Count == 0)
            return;

        var resourceToLoad = _loadQueue.Dequeue();

        if (_loadedResources.TryGetValue(resourceToLoad, out var cachedResource))
        {
            LoadCompleted?.Invoke(resourceToLoad, cachedResource);
            return;
        }

        _currentLoadingResource = resourceToLoad;

        try
        {
            var result = ResourceLoader.LoadThreadedRequest(resourceToLoad);

            if (result != Error.Ok)
            {
                GD.PrintErr($"Failed to request resource '{resourceToLoad}': {result}");
                LoadFailed?.Invoke(resourceToLoad, $"Failed to load resource: {result}");
                _currentLoadingResource = null;
            }
            else
            {
                LoadStarted?.Invoke(resourceToLoad);
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Exception while requesting resource '{resourceToLoad}': {ex.Message}");
            LoadFailed?.Invoke(resourceToLoad, $"Exception: {ex.Message}");
            _currentLoadingResource = null;
        }
    }

    private void CheckLoadingStatus()
    {
        if (_currentLoadingResource == null)
            return;

        try
        {
            var status = ResourceLoader.LoadThreadedGetStatus(_currentLoadingResource, _progress);

            switch (status)
            {
                case ResourceLoader.ThreadLoadStatus.InProgress:
                    LoadProgress?.Invoke(_currentLoadingResource, (float)_progress[0]);
                    break;

                case ResourceLoader.ThreadLoadStatus.Loaded:
                    var resource = ResourceLoader.LoadThreadedGet(_currentLoadingResource);
                    if (resource != null)
                    {
                        _loadedResources[_currentLoadingResource] = resource;
                        LoadCompleted?.Invoke(_currentLoadingResource, resource);
                    }
                    else
                    {
                        LoadFailed?.Invoke(_currentLoadingResource, "Failed to retrieve loaded resource.");
                    }

                    _currentLoadingResource = null;
                    break;

                case ResourceLoader.ThreadLoadStatus.Failed:
                    LoadFailed?.Invoke(_currentLoadingResource, "Error during resource loading.");
                    _currentLoadingResource = null;
                    break;

                case ResourceLoader.ThreadLoadStatus.InvalidResource:
                    LoadFailed?.Invoke(_currentLoadingResource, "Invalid resource.");
                    _currentLoadingResource = null;
                    break;
                    
                default:
                    GD.PrintErr($"Unexpected load status for resource '{_currentLoadingResource}': {status}");
                    break;
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Exception while checking status for resource '{_currentLoadingResource}': {ex.Message}");
            if (_currentLoadingResource != null)
                LoadFailed?.Invoke(_currentLoadingResource, $"Exception: {ex.Message}");
            _currentLoadingResource = null;
        }
    }
}