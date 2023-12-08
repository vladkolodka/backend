using Convey;
using System;

namespace Menchul.Infrastructure.Exceptions
{
    public static class Extensions
    {
        public static string GetExceptionCode(this Exception exception)
            => exception.GetType().Name.Underscore().Replace("_exception", string.Empty);
    }
}