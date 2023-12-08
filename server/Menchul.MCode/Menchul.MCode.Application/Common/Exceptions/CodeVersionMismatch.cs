using Menchul.Application.Exceptions;

namespace Menchul.MCode.Application.Common.Exceptions
{
    public class CodeVersionMismatch : AppException
    {
        public CodeVersionMismatch(int expectedVersion, int actualVersion) : base(
            $"Code version should be [{expectedVersion}] but got [{actualVersion}].")
        {
        }
    }
}