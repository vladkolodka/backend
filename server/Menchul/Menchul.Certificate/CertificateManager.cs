using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Menchul.Certificate
{
    public class CertificateManager : ICertificateManager
    {
        private readonly ILogger<CertificateManager> _logger;

        public CertificateManager(ILogger<CertificateManager> logger)
        {
            _logger = logger;
        }

        private static X509KeyStorageFlags X509KeyStorageFlags
        {
            get
            {
                var keyStorageFlags = X509KeyStorageFlags.DefaultKeySet;

                // https://stackoverflow.com/questions/50340712/avoiding-the-keychain-when-using-x509certificate2-on-os-x
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    keyStorageFlags = X509KeyStorageFlags.Exportable;
                }

                return keyStorageFlags;
            }
        }

        public async Task InstallPfxCertificateAsync(string path, string password, X509Store store = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "Unable to create certificate from provided arguments.");
            }

            store ??= PersonalStore;

            _logger.LogInformation(
                "Installing certificate from '{CertPath}' to '{CertStoreName}' certificate store (location: {CertStoreLocation})",
                path, store.Name, store.Location);

            var certBytes = await File.ReadAllBytesAsync(path);

            InstallPfxCertificate(certBytes, password, store);
        }

        public void InstallPfxCertificate(byte[] certBytes, string password, X509Store store = null)
        {
            var cert = string.IsNullOrEmpty(password)
                ? new X509Certificate2(certBytes)
                : new X509Certificate2(
                    certBytes,
                    password,
                    X509KeyStorageFlags);

            AddToStore(cert, store);
        }

        public X509Store PersonalStore => new(StoreName.My, StoreLocation.CurrentUser);

        private void AddToStore(X509Certificate2 cert, X509Store store)
        {
            store.Open(OpenFlags.ReadWrite);
            store.Add(cert);

            var thumbprint = cert.Thumbprint ?? string.Empty;

            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            if (certificates.Count <= 0)
            {
                throw new ArgumentNullException(nameof(cert), "Unable to validate certificate was added to store.");
            }

            _logger.LogInformation("The certificate with thumbprint {CertThumbprint} added to store '{CertStoreName}'",
                cert.Thumbprint, store.Name);

            store.Close();
        }
    }
}