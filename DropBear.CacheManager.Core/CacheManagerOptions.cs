namespace DropBear.CacheManager.Core;
public class CacheManagerOptions
{
    public bool EnableInMemoryCache { get; set; } = true;
    public bool EnableFasterKvCache { get; set; } = true;
    public bool EnableDiskCache { get; set; } = true;
    public bool EnableSQLiteCache { get; set; } = true;

    public string? SQLiteDatabaseName { get; set; } = null;
    public string? DiskCachePath { get; set; } = null;
}
