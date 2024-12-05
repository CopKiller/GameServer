using System.Diagnostics;
using AutoMapper;
using AutoMapper.Internal;
using Core.Database.Models.Player;
using Core.Network.SerializationObjects;
using Core.Utils.AutoMapper;
using Core.Utils.AutoMapper.Interface;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Server.AutoMapper;

public class TestAutoMapper
{
    private ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Registro de AutoMapper e serviços relacionados
        services.AddAutoMapper(typeof(MappingProfile), typeof(Profile1), typeof(Profile2));
        services.AddScoped<IMapperService, MapperService>();

        // Registro de serviços auxiliares
        services.AddTransient<ISomeService>(sp => new FooService(5));

        return services.BuildServiceProvider();
    }

    [Fact]
    public void AutoMapperConfiguration_Should_Be_Valid()
    {
        // Arrange
        var serviceProvider = ConfigureServices();

        // Act
        var mapper = serviceProvider.GetService<IMapper>();

        // Assert
        Assert.NotNull(mapper);
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void MapperService_Should_Map_PlayerModel_To_PlayerDto()
    {
        // Arrange
        var serviceProvider = ConfigureServices();
        var mapperService = serviceProvider.GetService<IMapperService>();
        Assert.NotNull(mapperService);

        var source = new PlayerModel
        {
            Id = 1,
            SlotNumber = 1,
            Name = "Test",
            Level = 1,
            Experience = 0,
            Gold = 0,
            Vitals = new Vitals(),
            Stats = new Stats(),
            Position = new Position(),
            AccountModelId = 1
        };

        // Act
        var destination = mapperService.Map<PlayerModel, PlayerDto>(source);

        // Assert
        Assert.NotNull(destination);
        Assert.Equal(source.Id, destination.Id);
        Assert.Equal(source.SlotNumber, destination.SlotNumber);
        Assert.Equal(source.Name, destination.Name);
        Assert.Equal(source.Level, destination.Level);
        Assert.Equal(source.Experience, destination.Experience);
        Assert.Equal(source.Gold, destination.Gold);
        Assert.NotNull(destination.Vitals);
        Assert.NotNull(destination.Stats);
        Assert.NotNull(destination.Position);
    }

    [Fact]
    public void AutoMapper_Should_Map_Using_Custom_Resolver()
    {
        // Arrange
        var serviceProvider = ConfigureServices();
        var mapper = serviceProvider.GetRequiredService<IMapper>();

        // Act
        var destination = mapper.Map<Dest2>(new Source2());

        // Assert
        Assert.NotNull(destination);
        Assert.Equal(5, destination.ResolvedValue); // FooService valor inicial é 5
    }

    [Fact]
    public void AutoMapper_Should_Log_All_Type_Maps_And_Services()
    {
        // Arrange
        var serviceProvider = ConfigureServices();
        var mapper = serviceProvider.GetRequiredService<IMapper>();

        // Act: Listar todos os mapeamentos
        var typeMaps = mapper.ConfigurationProvider.Internal().GetAllTypeMaps();
        foreach (var typeMap in typeMaps)
            Debug.WriteLine($"{typeMap.SourceType.Name} -> {typeMap.DestinationType.Name}");

        // Act: Listar todos os serviços registrados
        foreach (var service in serviceProvider.GetServices<IMapper>()) Debug.WriteLine(service.GetType().Name);

        // Assert
        Assert.NotEmpty(typeMaps);
    }
}

// Classes e configurações auxiliares
public class Profile1 : Profile
{
    public Profile1()
    {
        CreateMap<Source, Dest>();
    }
}

public class Profile2 : Profile
{
    public Profile2()
    {
        CreateMap<Source2, Dest2>()
            .ForMember(d => d.ResolvedValue, opt => opt.MapFrom<DependencyResolver>());
    }
}

public class DependencyResolver : IValueResolver<object, object, int>
{
    private readonly ISomeService _service;

    public DependencyResolver(ISomeService service)
    {
        _service = service;
    }

    public int Resolve(object source, object destination, int destMember, ResolutionContext context)
    {
        return _service.Modify(destMember);
    }
}

public interface ISomeService
{
    int Modify(int value);
}

public class FooService : ISomeService
{
    private readonly int _value;

    public FooService(int value)
    {
        _value = value;
    }

    public int Modify(int value)
    {
        return value + _value;
    }
}

public class Source
{
}

public class Dest
{
}

public class Source2
{
}

public class Dest2
{
    public int ResolvedValue { get; set; }
}