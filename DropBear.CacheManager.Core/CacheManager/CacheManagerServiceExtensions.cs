using Microsoft.Extensions.DependencyInjection;

namespace DropBear.CacheManager.Core
{
    public static class CacheManagerServiceExtensions
    {
        /// <summary>
        /// Adds the Cache Manager to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddInternalCacheManager(this IServiceCollection services) 
        { 
            try 
            {
                // Register the compressor
                services.AddLZMACompressor();

                // Register Easy Caching
                services.AddEasyCaching(options => 
                {
                    options.UseInMemory("mem_cache").WithMessagePack("msgpack_serializer").WithCompressor();        
                    options.UseFasterKv(config => {config.SerializerName = "msgpack_serializer";},"fasterkv_cache").WithMessagePack("msgpack_serializer").WithCompressor();    

                    //configure?.Invoke(options);
                    // This is used to configure the actual EasyCaching options,
                    // if you want to use it you need to add an Action<EasyCachingOptions> 
                    // configure parameter to the AddCacheManager method
                });

                // Register the cache manager
                services.AddSingleton<ICacheManager, CacheManager>();
            } 
            catch (Exception ex) 
            {
                throw;
            }

            // Return the services
            return services;
        }
    }
}
