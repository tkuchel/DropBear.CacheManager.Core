using Microsoft.Extensions.DependencyInjection;

namespace DropBear.CacheManager.Core;

public static class CacheManagerServiceExtensions
{
    public static IServiceCollection AddInternalCacheManager(this IServiceCollection services) { 

        // Register the compressor
        services.AddLZMACompressor();

        // Register Easy Caching
        services.AddEasyCaching(options => {
            options.UseInMemory("mem_cache").WithMessagePack("msgpack_serializer").WithCompressor();        
            options.UseFasterKv(config => {config.SerializerName = "msgpack_serializer";},"fasterkv_cache").WithMessagePack("msgpack_serializer").WithCompressor();    
        });

        // Register the cache manager
        services.AddSingleton<ICacheManager, CacheManager>();

        // Return the services
        return services;
    }
}
