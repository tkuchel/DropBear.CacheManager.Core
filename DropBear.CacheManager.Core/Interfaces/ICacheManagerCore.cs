using System;
using System.Threading.Tasks;

namespace DropBear.CacheManager.Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing cache.
    /// </summary>
    public interface ICacheManagerCore
    {
        /// <summary>
        /// Checks if a key exists in the specified cache.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <param name="cacheType">The type of the cache.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the key exists in the cache.</returns>
        public Task<bool> ExistsAsync(string key, CacheType cacheType);

        /// <summary>
        /// Adds a value to the specified cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="expiration">The expiration time span.</param>
        /// <param name="cacheType">The type of the cache.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the operation was successful.</returns>
        Task<bool> AddAsync<T>(string key, T value, TimeSpan expiration, CacheType cacheType);

        /// <summary>
        /// Retrieves a value from the specified cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value.</param>
        /// <param name="cacheType">The type of the cache.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved value, or the default value of T if the key does not exist in the cache.</returns>
        Task<T> GetAsync<T>(string key, CacheType cacheType);

        /// <summary>
        /// Removes a value from the specified cache.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <param name="cacheType">The type of the cache.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the operation was successful.</returns>
        Task<bool> RemoveAsync(string key, CacheType cacheType);
    }
}
