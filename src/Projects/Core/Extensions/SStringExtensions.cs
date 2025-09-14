using System;

namespace StardustSandbox.Core.Extensions
{
    public static class SStringExtensions
    {
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value[..maxChars] + "...";
        }

        public static string FirstCharToUpper(this string value)
        {
            return value switch
            {
                null => throw new ArgumentNullException(nameof(value)),
                "" => throw new ArgumentException($"{nameof(value)} cannot be empty", nameof(value)),
                _ => value[0].ToString().ToUpper() + value[1..]
            };
        }
    }
}
