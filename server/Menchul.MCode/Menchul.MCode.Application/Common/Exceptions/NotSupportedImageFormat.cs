using Menchul.Application.Exceptions;

namespace Menchul.MCode.Application.Common.Exceptions
{
    public class NotSupportedImageFormat : AppException
    {
        public NotSupportedImageFormat(string imageFormat) : base($"The image format [{imageFormat}] is not supported.")
        {
        }
    }
}