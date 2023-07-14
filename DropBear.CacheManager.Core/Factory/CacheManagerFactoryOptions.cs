using Microsoft.Extensions.Logging;

namespace DropBear.CacheManager.Core.Factory;

public class CacheManagerFactoryOptions
{
    public bool UseMemoryCache { get; set; }
    public bool UseFasterKvCache { get; set; }
    public bool UseDiskCache { get; set; }
    public bool UseSQLiteCache { get; set; }
    public string? DiskCacheBasePath { get; set; } = null;
    public string? SQLiteCacheBasePath { get; set; } = null;
    public string? SQLiteFileName { get; set; } = null;
    
    public LogLevel DefaultLoggingLevel { get; set; } = LogLevel.Warning;
}