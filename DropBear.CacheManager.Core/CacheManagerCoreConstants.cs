namespace DropBear.CacheManager.Core;

public static class CacheManagerCoreConstants
{
    public static class EasyCachingConstants
    {
        public static class BaseProviderOptions
        {
            public static readonly int MaxRdSecond = 120;
            public static readonly bool EnableLogging = true;
            public static readonly int SleepMs = 300;
            public static readonly int LockMs = 5000;
            public static readonly bool CacheNulls = false;
        }
    }
}