using AutoMapper;
using Core.Utils.AutoMapper.Interface;

namespace Core.Utils.AutoMapper;

/// <summary>
///   Mapper service.
///   This service is used to map objects from one type to another.
/// </summary>
/// <param name="mapper"></param>
public class MapperService(IMapper mapper) : IMapperService
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return mapper.Map<TSource, TDestination>(source);
    }

    public TDestination Map<TDestination>(object source)
    {
        return mapper.Map<TDestination>(source);
    }
}