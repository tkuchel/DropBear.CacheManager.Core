using System;
using System.Threading.Tasks;

namespace DropBear.CacheManager.Core
{
    /// <summary>
    /// Defines methods for managing cache.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Checks if key exists in memory cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> ExistsInMemoryCache(string key);

        /// <summary>
        /// Checks if key exists in FasterKV cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> ExistsInFasterKvCache(string key);

        /// <summary>
        /// Adds a value to the memory cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiration">The expiration time span.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> AddToMemoryCache<T>(string key, T value, TimeSpan expiration);

        /// <summary>
        /// Adds a value to the FasterKV cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiration">The expiration time span.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> AddToFasterKvCache<T>(string key, T value, TimeSpan expiration);

        /// <summary>
        /// Retrieves a value from the memory cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<T?> GetFromMemoryCache<T>(string key);

        /// <summary>
        /// Retrieves a value from the FasterKV cache.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<T?> GetFromFasterKvCache<T>(string key);

        /// <summary>
        /// Removes a value from the memory cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> RemoveFromMemoryCache(string key);

        /// <summary>
        /// Removes a value from the FasterKV cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> RemoveFromFasterKvCache(string key);
    }
}
