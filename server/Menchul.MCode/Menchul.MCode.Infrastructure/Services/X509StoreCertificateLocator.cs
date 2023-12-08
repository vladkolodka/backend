using Menchul.Base.Exceptions;
using Menchul.Certificate;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Menchul.MCode.Infrastructure.Services
{
    public class X509StoreCertificateLocator : ICertificateLocator
    {
        private readonly ILogger<X509StoreCertificateLocator> _logger;
        private readonly ICertificateManager _certificateManager;

        public X509StoreCertificateLocator(ILogger<X509StoreCertificateLocator> logger, ICertificateManager certificateManager)
        {
            _logger = logger;
            _certificateManager = certificateManager;
        }

        public X509Certificate2 GetCertificate(string thumbprint)
        {
            if (string.IsNullOrWhiteSpace(thumbprint))
            {
                throw new ArgumentNullException(thumbprint);
            }

            using var store = _certificateManager.PersonalStore;
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certificates = store.Certificates;

            if (certificates.Count == 0)
            {
                throw new CertificateNotFoundException(thumbprint);
            }

            X509Certificate2Collection findResult = certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            if (findResult.Count == 0)
            {
                throw new CertificateNotFoundException(thumbprint);
            }

            X509Certificate2 certificate = findResult[0];

            _logger.LogDebug("Loaded X509Certificate2 for thumbprint {CertificateThumbprint}", thumbprint);

            store.Close();

            return certificate;
        }
    }
}