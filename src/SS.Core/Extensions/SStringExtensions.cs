namespace StardustSandbox.Core.Extensions
{
    public static class SStringExtensions
    {
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value[..maxChars] + "...";
        }
    }
}
