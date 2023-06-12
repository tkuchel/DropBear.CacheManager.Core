using EasyCaching.Core;
using System;
using System.Threading.Tasks;

namespace DropBear.CacheManager.Core;

public class CacheManager : ICacheManager
{
    private readonly IEasyCachingProvider _memoryCacheProvider;
    private readonly IEasyCachingProvider _fasterKvCacheProvider;

    public CacheManager(IEasyCachingProviderFactory providerFactory)
    {
        _memoryCacheProvider = providerFactory.GetCachingProvider("mem_cache");
        _fasterKvCacheProvider = providerFactory.GetCachingProvider("fasterkv_cache");
    }

    // Check if key exists in memory cache
    public async Task<bool> ExistsInMemoryCache(string key)
    {
        return await _memoryCacheProvider.ExistsAsync(key);
    }

    // Check if key exists in FasterKV cache
    public async Task<bool> ExistsInFasterKvCache(string key)
    {
        return await _fasterKvCacheProvider.ExistsAsync(key);
    }

    // Add to memory cache
    public async Task<bool> AddToMemoryCache<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            await _memoryCacheProvider.SetAsync(key, value, expiration);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Retrieve from memory cache
    public async Task<T> GetFromMemoryCache<T>(string key)
    {
        try
        {
            var result = await _memoryCacheProvider.GetAsync<T>(key);
            return result.HasValue ? result.Value : default;
        }
        catch
        {
            // Log or handle the exception
            return default;
        }
    }
public async Task<T?> GetFromMemoryCacheOrDefault<T>(string key, T? defaultValue = default)
{
    try
    {
        var result = await _memoryCacheProvider.GetAsync<T>(key);
        return result.HasValue ? result.Value : defaultValue;
    }
    catch
    {
        // Log or handle the exception
        return defaultValue;
    }
}
    // Remove from memory cache
    public async Task<bool> RemoveFromMemoryCache(string key)
    {
        try
        {
            await _memoryCacheProvider.RemoveAsync(key);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Add to FasterKV cache
    public async Task<bool> AddToFasterKvCache<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            await _fasterKvCacheProvider.SetAsync(key, value, expiration);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Retrieve from FasterKV cache
    public async Task<T> GetFromFasterKvCache<T>(string key)
    {
        try
        {
            var result = await _fasterKvCacheProvider.GetAsync<T>(key);
            return result.HasValue ? result.Value : default;
        }
        catch
        {
            // Log or handle the exception
            return default;
        }
    }

    public async Task<T?> GetFromFasterKvCacheOrDefault<T>(string key, T? defaultValue = default)
{
    try
    {
        var result = await _fasterKvCacheProvider.GetAsync<T>(key);
        return result.HasValue ? result.Value : defaultValue;
    }
    catch
    {
        // Log or handle the exception
        return defaultValue;
    }
}

    // Remove from FasterKV cache
    public async Task<bool> RemoveFromFasterKvCache(string key)
    {
        try
        {
            await _fasterKvCacheProvider.RemoveAsync(key);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
