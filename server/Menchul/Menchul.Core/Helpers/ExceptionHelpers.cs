using System;

namespace Menchul.Core.Helpers
{
    public static class ExceptionHelpers
    {
        public static bool ThrowsAny<T>(Action action)
        {
            try
            {
                action.Invoke();
                return false;
            }
            catch (Exception e)
            {
                return e is T;
            }
        }

        public static bool TrueResult(Func<bool> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}