using DropBear.CacheManager.Core.Interfaces;
using EasyCaching.Core;
using EasyCaching.Core.Configurations;
using EasyCaching.Disk;
using EasyCaching.SQLite;
using Lamar;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Constants = DropBear.CacheManager.Core.CacheManagerCoreConstants;

namespace DropBear.CacheManager.Core.Factory;

public class CacheManagerFactory
{
    private static IContainer? _container;

    [Obsolete("This method is deprecated. Use Create(ILogger, IEasyCachingProvider memory, IEasyCachingProvider fasterKv, IEasyCachingProvider disk, IEasyCachingProvider sqlite) instead.")]
    public CacheManagerCore Create(Action<CacheManagerFactoryOptions> configure)
    {
        var options = new CacheManagerFactoryOptions();
        configure(options);

        if (_container == null)
            _container = new Container(x =>
            {
                x.AddLogging(configureLog => configureLog.AddConsole())
                    .Configure<LoggerFilterOptions>(filterOptions =>
                        filterOptions.MinLevel = options.DefaultLoggingLevel);

                // Register the compressor
                x.AddLZMACompressor();

                // Register Easy Caching
                x.AddEasyCaching(config =>
                {
                    // Configure Disk Cache
                    if (options.UseDiskCache) ConfigureDiskCache(options, config);

                    // Configure FasterKV Cache
                    if (options.UseFasterKvCache) ConfigureFasterKvCache(options, config);

                    // Configure Memory Cache
                    if (options.UseMemoryCache) ConfigureMemoryCache(options, config);

                    // Configure SQLite Cache
                    if (options.UseSQLiteCache) ConfigureSQLiteCache(options, config);
                });

                x.IncludeRegistry<CoreRegistry>();

                //x.For<ICacheManagerCore>().Use<CacheManagerCore>().Singleton();
                x.For<ICacheManagerCore>().Use(ctx =>
                    new CacheManagerCore(
                        ctx.GetInstance<IEasyCachingProviderFactory>(),
                        ctx.GetInstance<ILogger<CacheManagerCore>>()
                    )
                ).Singleton();
            });

        return _container.GetInstance<ICacheManagerCore>() as CacheManagerCore;
    }

    public CacheManagerCore Create(IEasyCachingProviderFactory providerFactory,
        Action<CacheManagerFactoryOptions> configure)
    {
        var options = new CacheManagerFactoryOptions();
        configure(options);

        IEasyCachingProvider memoryCacheProvider = null;
        IEasyCachingProvider fasterKvCacheProvider = null;
        IEasyCachingProvider diskCacheProvider = null;
        IEasyCachingProvider sqliteCacheProvider = null;

        if (_container == null)
            _container = new Container(x =>
            {
                x.AddLogging(configureLog => configureLog.AddConsole())
                    .Configure<LoggerFilterOptions>(filterOptions =>
                        filterOptions.MinLevel = options.DefaultLoggingLevel);

                // Register the compressor
                x.AddLZMACompressor();

                // Register Easy Caching
                x.AddEasyCaching(config =>
                {
                    // Configure Disk Cache
                    if (options.UseDiskCache)
                    {
                        ConfigureDiskCache(options, config);
                        diskCacheProvider = providerFactory.GetCachingProvider("disk_cache");
                    }

                    // Configure FasterKV Cache
                    if (options.UseFasterKvCache)
                    {
                        ConfigureFasterKvCache(options, config);
                        fasterKvCacheProvider = providerFactory.GetCachingProvider("fasterkv_cache");
                    }

                    // Configure Memory Cache
                    if (options.UseMemoryCache)
                    {
                        ConfigureMemoryCache(options, config);
                        memoryCacheProvider = providerFactory.GetCachingProvider("mem_cache");
                    }

                    // Configure SQLite Cache
                    if (options.UseSQLiteCache)
                    {
                        ConfigureSQLiteCache(options, config);
                        sqliteCacheProvider = providerFactory.GetCachingProvider("sqlite_cache");
                    }
                });

                x.IncludeRegistry<CoreRegistry>();


                // Use the new constructor that accepts the providerFactory
                x.For<ICacheManagerCore>().Use(ctx =>
                    new CacheManagerCore(
                        ctx.GetInstance<ILogger<CacheManagerCore>>(),
                        memoryCacheProvider, fasterKvCacheProvider, diskCacheProvider, sqliteCacheProvider
                    )
                ).Singleton();
            });

        return _container.GetInstance<ICacheManagerCore>() as CacheManagerCore;
    }

    private void ConfigureDiskCache(CacheManagerFactoryOptions options, EasyCachingOptions config)
    {
        var diskCacheBasePath = options.DiskCacheBasePath ?? GetUniquePath();
        config.UseDisk(configuration =>
            {
                configuration.DBConfig = new DiskDbOptions
                {
                    BasePath = diskCacheBasePath
                };
                ApplyBaseProviderOptions(configuration);
            }, "disk_cache")
            .WithMessagePack("msgpack_serializer")
            .WithCompressor();
    }

    private void ConfigureFasterKvCache(CacheManagerFactoryOptions options, EasyCachingOptions config)
    {
        config.UseFasterKv(configuration => { ApplyBaseProviderOptions(configuration); }, "fasterkv_cache")
            .WithMessagePack("msgpack_serializer")
            .WithCompressor();
    }

    private void ConfigureMemoryCache(CacheManagerFactoryOptions options, EasyCachingOptions config)
    {
        config.UseInMemory(configuration => { ApplyBaseProviderOptions(configuration); }, "mem_cache")
            .WithMessagePack("msgpack_serializer")
            .WithCompressor();
    }

    private void ConfigureSQLiteCache(CacheManagerFactoryOptions options, EasyCachingOptions config)
    {
        var sqliteCacheBasePath = options.SQLiteCacheBasePath ?? GetUniquePath();
        var sqliteFileName = options.SQLiteFileName ?? "cache.db";

        config.UseSQLite(configuration =>
            {
                configuration.DBConfig = new SQLiteDBOptions
                {
                    FileName = sqliteFileName,
                    FilePath = sqliteCacheBasePath,
                    CacheMode = SqliteCacheMode.Default,
                    OpenMode = SqliteOpenMode.ReadWriteCreate
                };
                ApplyBaseProviderOptions(configuration);
            }, "sqlite_cache")
            .WithMessagePack("msgpack_serializer")
            .WithCompressor();
    }

    private void ApplyBaseProviderOptions(BaseProviderOptions configuration)
    {
        configuration.SerializerName = "msgpack_serializer";
        configuration.CacheNulls = CacheManagerCoreConstants.EasyCachingConstants.BaseProviderOptions.CacheNulls;
        configuration.SleepMs = CacheManagerCoreConstants.EasyCachingConstants.BaseProviderOptions.SleepMs;
        configuration.LockMs = CacheManagerCoreConstants.EasyCachingConstants.BaseProviderOptions.LockMs;
        configuration.EnableLogging = CacheManagerCoreConstants.EasyCachingConstants.BaseProviderOptions.EnableLogging;
        configuration.MaxRdSecond = CacheManagerCoreConstants.EasyCachingConstants.BaseProviderOptions.MaxRdSecond;
    }

    private string GetUniquePath()
    {
        var uniquePath = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(uniquePath);
        return uniquePath;
    }

    private class CoreRegistry : ServiceRegistry
    {
        public CoreRegistry()
        {
            Scan(scan =>
            {
                scan.AssemblyContainingType<CacheManagerCore>();
                scan.WithDefaultConventions();
            });
        }
    }
}