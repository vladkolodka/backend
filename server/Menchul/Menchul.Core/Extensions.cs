using System;

namespace Menchul.Core
{
    public static class Extensions
    {
        public static bool IsWhiteSpace(this string s) => !string.IsNullOrEmpty(s) && string.IsNullOrWhiteSpace(s);

        public static string ToCamelCase(this string s) => string.IsNullOrWhiteSpace(s)
            ? string.Empty
            : Char.ToLowerInvariant(s[0]) + s.Substring(1);
    }
}