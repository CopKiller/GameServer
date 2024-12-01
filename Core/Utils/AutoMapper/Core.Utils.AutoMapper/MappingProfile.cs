using AutoMapper;
using Core.Database.Interfaces.Account;
using Core.Database.Interfaces.Player;
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
        CreateMap<AccountModel, AccountDto>();
        CreateMap<PlayerModel, PlayerDto>();
        CreateMap<Stats, StatsDto>();
        CreateMap<Vitals, VitalsDto>();
        CreateMap<Position, PositionDto>();

        // Mapeamentos para interfaces
        CreateMap<IAccountModel, AccountDto>();
        CreateMap<IPlayerModel, PlayerDto>();
        CreateMap<IStats, StatsDto>();
        CreateMap<IVitals, VitalsDto>();
        CreateMap<IPosition, PositionDto>();
    }
}