using Menchul.Core.Helpers;
using System.Globalization;

namespace Menchul.MCode.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static bool IsValidIsoLanguageName(this string languageName) =>
            ExceptionHelpers.TrueResult(() =>
                CultureInfo.GetCultureInfo(languageName).TwoLetterISOLanguageName == languageName);
    }
}