using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Menchul.Certificate
{
    public interface ICertificateManager
    {
        Task InstallPfxCertificateAsync(string path, string password, X509Store store = null);
        void InstallPfxCertificate(byte[] certBytes, string password, X509Store store = null);

        X509Store PersonalStore { get; }
    }
}