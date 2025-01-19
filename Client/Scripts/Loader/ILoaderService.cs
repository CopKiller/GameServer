using System;
using Godot;

namespace Game.Scripts.Loader;

public interface ILoaderService
{
    event Action<string> LoadStarted;
    event Action<string, float> LoadProgress;
    event Action<string, Resource> LoadCompleted;
    event Action<string, string> LoadFailed;

    void EnqueueResource(string resourcePath, LoaderPriority priority);
    void ProcessQueue();
}