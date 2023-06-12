namespace DropBear.CacheManager.Core;
public interface ICacheManager
{
Task<bool> ExistsInMemoryCache(string key);
Task<bool> ExistsInFasterKvCache(string key);
Task<bool> AddToMemoryCache<T>(string key, T value, TimeSpan expiration);
Task<T> GetFromMemoryCache<T>(string key);
Task<T?> GetFromMemoryCacheOrDefault<T>(string key, T? defaultValue = default);
Task<bool> RemoveFromMemoryCache(string key);
Task<bool> AddToFasterKvCache<T>(string key, T value, TimeSpan expiration);
Task<T> GetFromFasterKvCache<T>(string key);
Task<T?> GetFromFasterKvCacheOrDefault<T>(string key, T? defaultValue = default);
Task<bool> RemoveFromFasterKvCache(string key);
}
