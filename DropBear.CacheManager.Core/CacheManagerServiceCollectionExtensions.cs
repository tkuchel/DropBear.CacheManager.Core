using Microsoft.Extensions.DependencyInjection;
using System;
using DropBear.CacheManager.Core.CacheManager;
using DropBear.CacheManager.Core.PreFlight;

namespace DropBear.CacheManager.Core
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/> to add CacheManager services.
    /// </summary>
    public static class CacheManagerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the CacheManager services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configure">A delegate to configure the <see cref="CacheManagerOptions"/>. If null, default options will be used.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddCacheManager(this IServiceCollection services, Action<CacheManagerOptions>? configure = null)
        {
            // Check for nulls
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Create CacheManagerOptions and register them
            var options = new CacheManagerOptions();
            configure?.Invoke(options);
            services.AddSingleton(options);

            // Register the cache manager
            services.AddInternalCacheManager();

            // Run Preflight checks
            services.AddHostedService<PreflightCheckService>();

            // Return the services
            return services;
        }
    }
}
