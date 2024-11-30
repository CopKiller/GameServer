namespace Core.Utils.AutoMapper.Interface;

public interface IMapperService
{
    TDestination Map<TSource, TDestination>(TSource source);
    TDestination Map<TDestination>(object source);
}