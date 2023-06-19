using EasyCaching.Core;
using Microsoft.Extensions.Logging;

namespace DropBear.CacheManager.Core.CacheManager
{
    /// <summary>
    /// Provides methods for managing cache.
    /// </summary>
    public class CacheManager : ICacheManager
    {
        private readonly IEasyCachingProvider _memoryCacheProvider;
        private readonly IEasyCachingProvider _fasterKvCacheProvider;
        private readonly IEasyCachingProvider _diskCacheProvider;
        private readonly IEasyCachingProvider _sqliteCacheProvider;
        private readonly ILogger<CacheManager> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        /// <param name="providerFactory">The provider factory.</param>
        public CacheManager(
            IEasyCachingProviderFactory providerFactory,
            ILogger<CacheManager> logger
        )
        {
            _memoryCacheProvider = providerFactory.GetCachingProvider("mem_cache");
            _fasterKvCacheProvider = providerFactory.GetCachingProvider("fasterkv_cache");
            _diskCacheProvider = providerFactory.GetCachingProvider("disk_cache");
            _sqliteCacheProvider = providerFactory.GetCachingProvider("sqlite_cache");
            _logger = logger;
        }

        /// <summary>
        /// Checks if key exists in memory cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> ExistsInMemoryCache(string key)
        {
            return ExistsInCache(_memoryCacheProvider, key);
        }

        /// <summary>
        /// Checks if key exists in FasterKV cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> ExistsInFasterKvCache(string key)
        {
            return ExistsInCache(_fasterKvCacheProvider, key);
        }

        private async Task<bool> ExistsInCache(IEasyCachingProvider provider, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            try
            {
                return await provider.ExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while checking if key '{Key}' exists in cache",
                    key
                );
                throw;
            }
        }

        // Disk Cache

        /// <summary>
        /// Checks if key exists in disk cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> ExistsInDiskCache(string key)
        {
            return ExistsInCache(_diskCacheProvider, key);
        }

        /// <summary>
        /// Adds a value to the disk cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiration">The expiration time span.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> AddToDiskCache<T>(string key, T value, TimeSpan expiration)
        {
            return AddToCache(_diskCacheProvider, key, value, expiration);
        }

        /// <summary>
        /// Retrieves a value from the disk cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<T?> GetFromDiskCache<T>(string key)
        {
            return GetFromCache<T>(_diskCacheProvider, key);
        }

        /// <summary>
        /// Removes a value from the disk cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> RemoveFromDiskCache(string key)
        {
            return RemoveFromCache(_diskCacheProvider, key);
        }

        // SQLite Cache

        /// <summary>
        /// Checks if key exists in SQLite cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> ExistsInSQLiteCache(string key)
        {
            return ExistsInCache(_sqliteCacheProvider, key);
        }

        /// <summary>
        /// Adds a value to the SQLite cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiration">The expiration time span.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> AddToSQLiteCache<T>(string key, T value, TimeSpan expiration)
        {
            return AddToCache(_sqliteCacheProvider, key, value, expiration);
        }

        /// <summary>
        /// Retrieves a value from the SQLite cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<T?> GetFromSQLiteCache<T>(string key)
        {
            return GetFromCache<T>(_sqliteCacheProvider, key);
        }

        /// <summary>
        /// Removes a value from the SQLite cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> RemoveFromSQLiteCache(string key)
        {
            return RemoveFromCache(_sqliteCacheProvider, key);
        }

        /// <summary>
        /// Adds a value to the memory cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiration">The expiration time span.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> AddToMemoryCache<T>(string key, T value, TimeSpan expiration)
        {
            return AddToCache(_memoryCacheProvider, key, value, expiration);
        }

        /// <summary>
        /// Adds a value to the FasterKV cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiration">The expiration time span.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> AddToFasterKvCache<T>(string key, T value, TimeSpan expiration)
        {
            return AddToCache(_fasterKvCacheProvider, key, value, expiration);
        }

        private async Task<bool> AddToCache<T>(
            IEasyCachingProvider provider,
            string key,
            T value,
            TimeSpan expiration
        )
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            try
            {
                await provider.SetAsync(key, value, expiration);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while adding the key '{Key}' into the cache",
                    key
                );
                throw;
            }
        }

        /// <summary>
        /// Retrieves a value from the memory cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<T?> GetFromMemoryCache<T>(string key)
        {
            return GetFromCache<T>(_memoryCacheProvider, key);
        }

        /// <summary>
        /// Retrieves a value from the FasterKV cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<T?> GetFromFasterKvCache<T>(string key)
        {
            return GetFromCache<T>(_fasterKvCacheProvider, key);
        }

        private async Task<T?> GetFromCache<T>(IEasyCachingProvider provider, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            try
            {
                var result = await provider.GetAsync<T>(key);
                return result.HasValue ? result.Value : default;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while retrieving the key '{Key}' from the cache",
                    key
                );
                throw;
            }
        }

        /// <summary>
        /// Removes a value from the memory cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> RemoveFromMemoryCache(string key)
        {
            return RemoveFromCache(_memoryCacheProvider, key);
        }

        /// <summary>
        /// Removes a value from the FasterKV cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<bool> RemoveFromFasterKvCache(string key)
        {
            return RemoveFromCache(_fasterKvCacheProvider, key);
        }

        private async Task<bool> RemoveFromCache(IEasyCachingProvider provider, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            try
            {
                await provider.RemoveAsync(key);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while removing the key '{Key}' from the cache",
                    key
                );
                throw;
            }
        }
    }
}
