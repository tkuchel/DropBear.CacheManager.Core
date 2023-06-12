namespace DropBear.CacheManager.Core;

public static class ArgumentValidator
{
    public static void NotNull(object argument, string argumentName)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(argumentName);
        }
    }

    public static void NotNull(object argument, string argumentName, bool allowNulls)
    {
        if (argument == null && !allowNulls)
        {
            throw new ArgumentNullException(argumentName);
        }
    }

    public static void NotNullOrWhiteSpace(string argument, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentNullException(argumentName);
        }
    }

    public static void NotNegativeOrZero(TimeSpan argument, string argumentName)
    {
        if (argument <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(argumentName);
        }
    }

    public static void NotNullAndCountGTZero<T>(IEnumerable<T> argument, string argumentName)
    {
        if (argument == null || !argument.Any())
        {
            throw new ArgumentNullException(argumentName);
        }
    }

    public static void NotNullAndCountGTZero<T>(
        IDictionary<string, T> argument,
        string argumentName
    )
    {
        if (argument == null || argument.Count <= 0)
        {
            throw new ArgumentNullException(argumentName);
        }
    }
}
