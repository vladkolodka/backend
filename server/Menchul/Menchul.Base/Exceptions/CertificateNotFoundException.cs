using System;

namespace Menchul.Base.Exceptions
{
    public class CertificateNotFoundException : ApplicationException
    {
        public CertificateNotFoundException(string thumbprint) : base(
            $"Certificate with Thumbprint [{thumbprint}] does not found. Please check thumbprint.")
        {
        }
    }
}