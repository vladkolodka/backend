using System;

namespace Menchul.MCode.Application.Services.Hash
{
    public class InvalidHashFormatConfigurationException : ApplicationException
    {
        public InvalidHashFormatConfigurationException(string version, string message = null) : base(message)
        {
        }
    }
}