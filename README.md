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
await cacheManager.AddToCache("key", "value", TimeSpan.FromMinutes(5),CacheType.FasterKV);

// Check if key exists in cache
bool exists = await cacheManager.ExistsInCache("key",CacheType.FasterKV);

// Retrieve from cache
string value = await cacheManager.GetFromCache<string>("key",CacheType.FasterKV);

// Remove from cache
await cacheManager.RemoveFromCache("key",CacheType.FasterKV);
```

Replace `CacheType` with the name of the cache provider you want to use (`,CacheType.Memory`, `,CacheType.Disk`, `",CacheType.SQlite"`, or `",CacheType.FasterKV"`).

### Test Use
```csharp
void Main()
{
	TestCacheManagerCore().Wait();
}

public async Task TestCacheManagerCore()
{
	var factory = new CacheManagerFactory();
	var cacheManager = factory.Create(options =>
	{
		options.UseMemoryCache = true;
		options.UseDiskCache = true;
		options.UseSQLiteCache = true;
		options.UseFasterKvCache = true;
		options.DefaultLoggingLevel = LogLevel.Debug;
	});

	// Add to cache
	await cacheManager.AddAsync("key", "value", TimeSpan.FromMinutes(5),CacheType.FasterKV);
	Console.WriteLine("Item added to cache.");

	// Check if key exists in cache
	bool exists = await cacheManager.ExistsAsync("key",CacheType.FasterKV);
	Console.WriteLine($"Item exists in cache: {exists}");

	// Retrieve from cache
	string value = await cacheManager.GetAsync<string>("key",CacheType.FasterKV);
	Console.WriteLine($"Retrieved item from cache: {value}");

	// Remove from cache
	await cacheManager.RemoveAsync("key",CacheType.FasterKV);
	Console.WriteLine("Item removed from cache.");

	// Check if key exists in cache
	exists = await cacheManager.ExistsAsync("key",CacheType.FasterKV);
	Console.WriteLine($"Item exists in cache: {exists}");
	
	return;
}
```

## License

This project is licensed under the MIT License.
