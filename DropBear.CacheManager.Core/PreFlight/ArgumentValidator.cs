using System;
using System.Collections.Generic;
using System.Linq;

namespace DropBear.CacheManager.Core
{
    /// <summary>
    /// Provides methods for validating arguments.
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// Ensures that the specified argument is not null.
        /// </summary>
        /// <param name="argument">The argument to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public static void NotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName, $"{argumentName} cannot be null.");
            }
        }

        /// <summary>
        /// Ensures that the specified argument is not null or whitespace.
        /// </summary>
        /// <param name="argument">The argument to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public static void NotNullOrWhiteSpace(string argument, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentNullException(argumentName, $"{argumentName} cannot be null or whitespace.");
            }
        }

        /// <summary>
        /// Ensures that the specified argument is not null and has a count greater than zero.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="argument">The argument to validate.</param>
        /// <param name="argumentName">The name of the argument.</param>
        public static void NotNullAndCountGTZero<T>(IEnumerable<T> argument, string argumentName)
        {
            if (argument == null || !argument.Any())
            {
                throw new ArgumentNullException(argumentName, $"{argumentName} cannot be null and must have at least one item.");
            }
        }
    }
}
