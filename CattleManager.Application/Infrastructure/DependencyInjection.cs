using CattleManager.Application.Application.Common.Interfaces.Entities.Breeds;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;
using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccines;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Infrastructure.Persistence;
using CattleManager.Application.Infrastructure.Providers;

namespace CattleManager.Application.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICattleRepository, CattleRepository>();
        services.AddScoped<IMilkProductionRepository, MilkProductionRepository>();
        services.AddScoped<IVaccinationRepository, VaccinationRepository>();
        services.AddScoped<IVaccineRepository, VaccineRepository>();
        services.AddScoped<IConceptionRepository, ConceptionRepository>();
        services.AddScoped<IBreedRepository, BreedRepository>();
        services.AddSingleton<IGuidProvider, GuidProvider>();

        return services;
    }
}