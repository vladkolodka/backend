using Menchul.Application.Exceptions;

namespace Menchul.MCode.Application.ProductUnits.Exceptions
{
    public class EanInvalid : AppException
    {
        public EanInvalid() : base("EAN code invalid.")
        {
        }
    }
}