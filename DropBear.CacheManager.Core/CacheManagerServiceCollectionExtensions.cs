using DropBear.CacheManager.Core.Factory;
using DropBear.CacheManager.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DropBear.CacheManager.Core;

/// <summary>
///     Provides extension methods for <see cref="IServiceCollection" /> to add CacheManager services.
/// </summary>
public static class CacheManagerServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the CacheManager services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configure">
    ///     A delegate to configure the <see cref="CacheManagerFactoryOptions" />. If null, default options
    ///     will be used.
    /// </param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddCacheManager(this IServiceCollection services,
        Action<CacheManagerFactoryOptions>? configure = null)
    {
        // Check for nulls
        if (services == null) throw new ArgumentNullException(nameof(services));

        // Create CacheManagerCore using CacheManagerFactory and register it
        var factory = new CacheManagerFactory();
        var cacheManagerCore = factory.Create(configure);
        services.AddSingleton<ICacheManagerCore>(cacheManagerCore);

        // Return the services
        return services;
    }
}