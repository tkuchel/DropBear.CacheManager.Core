using EasyCaching.Disk;
using EasyCaching.SQLite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DropBear.CacheManager.Core
{
    public static class CacheManagerServiceExtensions
    {
        /// <summary>
        /// Adds the Cache Manager to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="injectedOptions">The injected options.</param>
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
                    var injectedOptions = services
                        .BuildServiceProvider()
                        .GetRequiredService<CacheManagerOptions>();

                    if (injectedOptions.EnableInMemoryCache)
                        options
                            .UseInMemory("mem_cache")
                            .WithMessagePack("msgpack_serializer")
                            .WithCompressor();

                    if (injectedOptions.EnableFasterKvCache)
                        options
                            .UseFasterKv(
                                config =>
                                {
                                    config.SerializerName = "msgpack_serializer";
                                },
                                "fasterkv_cache"
                            )
                            .WithMessagePack("msgpack_serializer")
                            .WithCompressor();

                    if (injectedOptions.EnableDiskCache)
                        options
                            .UseDisk(configuration =>
                            {
                                configuration.DBConfig = new DiskDbOptions
                                {
                                    BasePath = Path.GetTempPath()
                                };
                            })
                            .WithMessagePack("msgpack_serializer")
                            .WithCompressor();

                    if (injectedOptions.EnableSQLiteCache)
                        options
                            .UseSQLite(
                                config =>
                                {
                                    config.DBConfig = new SQLiteDBOptions
                                    {
                                        FileName = "sqlite_cache.db"
                                    };
                                },
                                "sqlite_cache"
                            )
                            .WithMessagePack("msgpack_serializer")
                            .WithCompressor();

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
