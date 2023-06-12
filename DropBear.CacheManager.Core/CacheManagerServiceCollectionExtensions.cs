using Microsoft.Extensions.DependencyInjection;

namespace DropBear.CacheManager.Core;
public static class CacheManagerServiceCollectionExtensions
{
    public static IServiceCollection AddCacheManager(this IServiceCollection services, Action<CacheManagerOptions> configure)
    {
        // Check for nulls
        ArgumentValidator.NotNull(configure, nameof(configure));

        // Register the options
        services.Configure(configure);

        // Register the cache manager
        services.AddCacheManager();

        // Run Preflight checks
        services.AddHostedService<PreflightCheckService>();

        // Return the services
        return services;
    }
}
