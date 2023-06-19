# CacheManager 

CacheManager is a .NET library that provides an easy-to-use caching abstraction with support for in-memory, FasterKV, Disk and SQLite caches. 

## Features 

- Simple API for adding, retrieving, and removing items from cache. 
- Support for both in-memory and FasterKV caches. 
- Support for disk and SQLite caches (new).
- Built-in preflight checks to ensure proper configuration. 
- Extensible design that allows for additional cache providers. 
  
## Installation 

You can add CacheManager to your project by using the NuGet package manager in Visual Studio, or by using the `dotnet add package` command in the .NET CLI: 

```bash 
dotnet add package DropBear.CacheManager 
``` 

## Usage 

First, register the CacheManager services in your `Startup.cs` file: 

```csharp 
public void ConfigureServices(IServiceCollection services) 
{ 
    // Other service configuration... 

    services.AddCacheManager(); 
} 
``` 

Then, you can inject and use `ICacheManager` in your classes: 

```csharp 
public class MyClass 
{ 
    private readonly ICacheManager _cacheManager; 

    public MyClass(ICacheManager cacheManager) 
    { 
        _cacheManager = cacheManager; 
    } 

    public async Task MyMethod() 
    { 
        // Use the cache manager 
        bool exists = await _cacheManager.ExistsInMemoryCache("myKey"); 
        // Other code... 
    } 
} 
``` 

## Documentation 

For more detailed documentation, please see the official documentation. 

## Contributing 

We welcome contributions! Please see our contributing guide for details. 

## License 

CacheManager is licensed under the MIT License. 
