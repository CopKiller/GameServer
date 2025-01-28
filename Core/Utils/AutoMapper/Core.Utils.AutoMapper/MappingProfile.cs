using AutoMapper;
using Core.Database.Interface.Account;
using Core.Database.Interface.Player;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Player;

namespace Core.Utils.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamentos para tipos concretos
        CreateMap<AccountModel, AccountDto>( )
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToString("yyyy-MM-dd")));
        CreateMap<PlayerModel, PlayerDto>();
        CreateMap<Stats, StatsDto>();
        CreateMap<Vitals, VitalsDto>();
        CreateMap<Position, Vector2>();
        
        // inverso
        // Converter o campo de birthdate em string para o DateOnly
        CreateMap<AccountDto, AccountModel>()
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.Parse(src.BirthDate)));
        CreateMap<PlayerDto, PlayerModel>();
        CreateMap<StatsDto, Stats>();
        CreateMap<VitalsDto, Vitals>();
        CreateMap<Vector2, Position>();

        // Mapeamentos para interfaces
        CreateMap<IAccountModel, AccountDto>();
        CreateMap<IPlayerModel, PlayerDto>();
        CreateMap<IStats, StatsDto>();
        CreateMap<IVitals, VitalsDto>();
        CreateMap<IPosition, Vector2>();
    }
}