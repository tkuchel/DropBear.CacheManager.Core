﻿using System.Xml.Serialization;
using EasyCaching.Disk;
using EasyCaching.SQLite;
using Microsoft.Extensions.DependencyInjection;

namespace DropBear.CacheManager.Core.CacheManager
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

                    if (injectedOptions == null)
                    {
                        // Log an error, or throw an exception
                        Console.WriteLine("injectedOptions is null");
                        throw new Exception("injectedOptions is null");
                    }

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
                                configuration.SerializerName = "msgpack_serializer";
                                configuration.DBConfig = new DiskDbOptions
                                {
                                    BasePath = injectedOptions.DiskCachePath ?? Path.GetTempPath(), 
                                };
                            }, "disk_cache")
                            .WithMessagePack("msgpack_serializer")
                            .WithCompressor();

                    if (injectedOptions.EnableSQLiteCache)
                        options
                            .UseSQLite(
                                config =>
                                {
                                    config.SerializerName = "msgpack_serializer";
                                    config.DBConfig = new SQLiteDBOptions
                                    {
                                        FileName = injectedOptions.SQLiteDatabaseName ?? "sqlite_cache.db" //
                                        
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
                services.AddSingleton<ICacheManager, Core.CacheManager.CacheManager>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            // Return the services
            return services;
        }
    }
}
