using Xunit;
using Moq;
using EasyCaching.Core;
using DropBear.CacheManager.Core;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class CacheManagerTests
{
    [Fact]
    public async Task ExistsInMemoryCache_WhenKeyExists_ReturnsTrue()
    {
        var key = "testKey";
        var mockLogger = new Mock<ILogger<CacheManager>>();
        var mockMemoryProvider = new Mock<IEasyCachingProvider>();
        mockMemoryProvider.Setup(x => x.ExistsAsync(key,default)).ReturnsAsync(true);

        var mockProviderFactory = new Mock<IEasyCachingProviderFactory>();
        mockProviderFactory.Setup(x => x.GetCachingProvider("mem_cache")).Returns(mockMemoryProvider.Object);

        var cacheManager = new CacheManager(mockProviderFactory.Object, mockLogger.Object);

        var result = await cacheManager.ExistsInMemoryCache(key);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsInFasterKvCache_WhenKeyExists_ReturnsTrue()
    {
        var key = "testKey";
        var mockLogger = new Mock<ILogger<CacheManager>>();
        var mockFasterKvProvider = new Mock<IEasyCachingProvider>();
        mockFasterKvProvider.Setup(x => x.ExistsAsync(key,default)).ReturnsAsync(true);

        var mockProviderFactory = new Mock<IEasyCachingProviderFactory>();
        mockProviderFactory.Setup(x => x.GetCachingProvider("fasterkv_cache")).Returns(mockFasterKvProvider.Object);

        var cacheManager = new CacheManager(mockProviderFactory.Object, mockLogger.Object);

        var result = await cacheManager.ExistsInFasterKvCache(key);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsInDiskCache_WhenKeyExists_ReturnsTrue()
    {
        var key = "testKey";
        var mockLogger = new Mock<ILogger<CacheManager>>();
        var mockDiskProvider = new Mock<IEasyCachingProvider>();
        mockDiskProvider.Setup(x => x.ExistsAsync(key,default)).ReturnsAsync(true);

        var mockProviderFactory = new Mock<IEasyCachingProviderFactory>();
        mockProviderFactory.Setup(x => x.GetCachingProvider("disk_cache")).Returns(mockDiskProvider.Object);

        var cacheManager = new CacheManager(mockProviderFactory.Object, mockLogger.Object);

        var result = await cacheManager.ExistsInDiskCache(key);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsInSQLiteCache_WhenKeyExists_ReturnsTrue()
    {
        var key = "testKey";
        var mockLogger = new Mock<ILogger<CacheManager>>();
        var mockSQLiteProvider = new Mock<IEasyCachingProvider>();
        mockSQLiteProvider.Setup(x => x.ExistsAsync(key,default)).ReturnsAsync(true);

        var mockProviderFactory = new Mock<IEasyCachingProviderFactory>();
        mockProviderFactory.Setup(x => x.GetCachingProvider("sqlite_cache")).Returns(mockSQLiteProvider.Object);

        var cacheManager = new CacheManager(mockProviderFactory.Object, mockLogger.Object);

        var result = await cacheManager.ExistsInSQLiteCache(key);

        Assert.True(result);
    }
}
