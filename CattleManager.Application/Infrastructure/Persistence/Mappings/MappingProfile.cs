using AutoMapper;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Infrastructure.Persistence.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponse>().ReverseMap();
        CreateMap<User, RegisterUserRequest>().ReverseMap();

        CreateMap<MilkProduction, MilkProductionRequest>().ReverseMap();
        CreateMap<MilkProduction, MilkProductionResponse>().ReverseMap();
        CreateMap<EditMilkProductionRequest, MilkProduction>().ReverseMap();
    }
}