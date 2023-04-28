using CattleManager.Application.Application.Common.Interfaces.Authentication;
using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.Breeds;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;
using CattleManager.Application.Application.Common.Interfaces.Entities.Messages;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;
using CattleManager.Application.Application.Common.Interfaces.ServiceValidations;
using CattleManager.Application.Application.Common.Marker;
using CattleManager.Application.Application.Services.Authentication;
using CattleManager.Application.Application.Services.Authorization;
using CattleManager.Application.Application.Services.CommonValidations;
using CattleManager.Application.Application.Services.Entities;
using FluentValidation;

namespace CattleManager.Application.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ICattleService, CattleService>();
        services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
        services.AddScoped<IMilkProductionService, MilkProductionService>();
        services.AddScoped<IVaccinationService, VaccinationService>();
        services.AddScoped<IConceptionService, ConceptionService>();
        services.AddScoped<IBreedService, BreedService>();
        services.AddScoped<IMilkSaleService, MilkSaleService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IServiceValidations, ServiceValidations>();
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

        return services;
    }
}