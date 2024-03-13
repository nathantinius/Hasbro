
using HitPoints.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HitPoints.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPlayerCharacterRepository, PlayerCharacterRepository>();
        return services;
    }
}