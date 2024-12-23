namespace Core.Client.Resource;

public class ResourcesUploaded<T> where T : class
{
    private readonly Dictionary<EResource, T> _resources = new();
    private readonly Dictionary<EResource, Action> _disposeActions = new();

    public bool AddResource(EResource resourceType, T resource)
    {
        if (_resources.TryAdd(resourceType, resource))
        {
            if (resource is IDisposable disposable)
            {
                _disposeActions.Add(resourceType, disposable.Dispose);
            }
            else
            {
                _disposeActions.Add(resourceType, () => { });
            }
            return true;
        }
        return false;
    }

    public T? GetResource(EResource resourceType)
    {
        return _resources.GetValueOrDefault(resourceType);
    }

    public bool ContainsResource(EResource resourceType)
    {
        return _resources.ContainsKey(resourceType);
    }

    public bool RemoveResource(EResource resourceType)
    {
        if (_resources.Remove(resourceType))
        {
            if (_disposeActions.TryGetValue(resourceType, out var disposeAction))
            {
                disposeAction.Invoke();
                _disposeActions.Remove(resourceType);
            }
            return true;
        }
        return false;
    }

    public void RemoveResources()
    {
        foreach (var disposeAction in _disposeActions.Values)
        {
            disposeAction.Invoke();
        }
        _resources.Clear();
        _disposeActions.Clear();
    }
}
