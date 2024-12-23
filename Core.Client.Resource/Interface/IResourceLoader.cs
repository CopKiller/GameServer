namespace Core.Client.Resource.Interface;

public interface IResourceLoader
{
    T? Load<T>(string path) where T : class;
}