using System.Security.Cryptography.X509Certificates;

namespace Menchul.MCode.Infrastructure.Services.Interfaces
{
    public interface ICertificateLocator
    {
        public X509Certificate2 GetCertificate(string thumbprint);
    }
}