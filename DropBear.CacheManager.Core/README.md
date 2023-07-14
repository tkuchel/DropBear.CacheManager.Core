# DropBear CacheManager

This package provides a simple and flexible way to manage caching in your .NET applications. It supports multiple caching providers including In-Memory, Disk, SQLite, and FasterKV.

## Installation

Install the NuGet package in your project:

```shell
dotnet add package DropBear.CacheManager
```

## Usage

### Using the Service Collection Extension

In your `Startup.cs` or wherever you configure your services, add the following code:

```csharp
services.AddCacheManagerCore(options =>
{
    options.UseMemoryCache = true;
    options.UseDiskCache = true;
    options.UseSQLiteCache = true;
    options.UseFasterKvCache = true;
    options.DefaultLoggingLevel = LogLevel.Debug;
});
```

### Using the CacheManagerFactory

If you're not using a `ServiceCollection`, you can use the `CacheManagerFactory` to create an instance of `CacheManagerCore`:

```csharp
var factory = new CacheManagerFactory();
var cacheManager = factory.Create(options =>
{
    options.UseMemoryCache = true;
    options.UseDiskCache = true;
    options.UseSQLiteCache = true;
    options.UseFasterKvCache = true;
    options.DefaultLoggingLevel = LogLevel.Debug;
});
```

### Basic Operations

The `CacheManagerCore` provides methods for adding, retrieving, and removing cache items:

```csharp
// Add to cache
await cacheManager.AddToCache("providerName", "key", "value", TimeSpan.FromMinutes(5));

// Check if key exists in cache
bool exists = await cacheManager.ExistsInCache("providerName", "key");

// Retrieve from cache
string value = await cacheManager.GetFromCache<string>("providerName", "key");

// Remove from cache
await cacheManager.RemoveFromCache("providerName", "key");
```

Replace `"providerName"` with the name of the cache provider you want to use (`"mem_cache"`, `"disk_cache"`, `"sqlite_cache"`, or `"fasterkv_cache"`).

## License

This project is licensed under the MIT License.
