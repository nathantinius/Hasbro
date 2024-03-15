
using FluentValidation;
using HitPoints.Application.Database;
using HitPoints.Application.Repositories;
using HitPoints.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HitPoints.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPlayerCharacterRepository, PlayerCharacterRepository>();
        services.AddSingleton<IPlayerCharacterService, PlayerCharacterService>();
        services.AddSingleton<IHitPointsService, HitPointService>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => 
            new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}