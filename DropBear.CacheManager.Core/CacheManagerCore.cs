using DropBear.CacheManager.Core.Interfaces;
using EasyCaching.Core;
using Microsoft.Extensions.Logging;

namespace DropBear.CacheManager.Core;

public class CacheManagerCore : ICacheManagerCore
{
    private readonly IEasyCachingProvider _diskCacheProvider;
    private readonly IEasyCachingProvider _fasterKvCacheProvider;
    private readonly ILogger<CacheManagerCore> _logger;
    private readonly IEasyCachingProvider _memoryCacheProvider;
    private readonly IEasyCachingProvider _sqliteCacheProvider;

    [Obsolete("This constructor is deprecated. Use the constructor with specific cache providers.")]
    public CacheManagerCore(
        IEasyCachingProviderFactory providerFactory,
        ILogger<CacheManagerCore> logger)
    {
        _memoryCacheProvider = providerFactory.GetCachingProvider("mem_cache");
        _fasterKvCacheProvider = providerFactory.GetCachingProvider("fasterkv_cache");
        _diskCacheProvider = providerFactory.GetCachingProvider("disk_cache");
        _sqliteCacheProvider = providerFactory.GetCachingProvider("sqlite_cache");
        _logger = logger;
    }
    
    public CacheManagerCore(
        ILogger<CacheManagerCore> logger,
        IEasyCachingProvider memoryCacheProvider = null,
        IEasyCachingProvider fasterKvCacheProvider = null,
        IEasyCachingProvider diskCacheProvider = null,
        IEasyCachingProvider sqliteCacheProvider = null)
    {
        _memoryCacheProvider = memoryCacheProvider;
        _fasterKvCacheProvider = fasterKvCacheProvider;
        _diskCacheProvider = diskCacheProvider;
        _sqliteCacheProvider = sqliteCacheProvider;
        _logger = logger;
    }

    public async Task<bool> ExistsAsync(string key, CacheType cacheType)
    {
        try
        {
            var provider = GetProvider(cacheType);
            return await provider.ExistsAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while checking if key '{Key}' exists in cache", key);
            throw;
        }
    }

    public async Task<bool> AddAsync<T>(string key, T value, TimeSpan expiration, CacheType cacheType)
    {
        try
        {
            var provider = GetProvider(cacheType);
            await provider.SetAsync(key, value, expiration);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding the key '{Key}' into the cache", key);
            throw;
        }
    }

    public async Task<T> GetAsync<T>(string key, CacheType cacheType)
    {
        try
        {
            var provider = GetProvider(cacheType);
            var result = await provider.GetAsync<T>(key);
            return result.HasValue ? result.Value : default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the key '{Key}' from the cache", key);
            throw;
        }
    }

    public async Task<bool> RemoveAsync(string key, CacheType cacheType)
    {
        try
        {
            var provider = GetProvider(cacheType);
            await provider.RemoveAsync(key);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while removing the key '{Key}' from the cache", key);
            throw;
        }
    }

    private IEasyCachingProvider GetProvider(CacheType cacheType)
    {
        return cacheType switch
        {
            CacheType.Memory => _memoryCacheProvider,
            CacheType.FasterKV => _fasterKvCacheProvider,
            CacheType.Disk => _diskCacheProvider,
            CacheType.SQLite => _sqliteCacheProvider,
            _ => throw new ArgumentOutOfRangeException(nameof(cacheType), cacheType, null)
        };
    }
}

public enum CacheType
{
    Memory,
    FasterKV,
    Disk,
    SQLite
}