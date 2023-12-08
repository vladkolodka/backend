using Convey.Security;
using Menchul.MCode.Application.Common.Configuration;
using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Application.Common.Exceptions;
using Menchul.MCode.Application.Hash;
using Menchul.MCode.Application.Services.Hash;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Menchul.MCode.Infrastructure.Services
{
    public class HashManager : IHashManager
    {
        private readonly ISigner _signer;
        private readonly ILogger<HashManager> _logger;
        private readonly X509Certificate2 _certificate;

        public HashManager(ISigner signer, ICertificateLocator certificateLocator,
            IOptions<QrGenerationOptions> qrOptions, ILogger<HashManager> logger)
        {
            _signer = signer;
            _logger = logger;
            _certificate = certificateLocator.GetCertificate(qrOptions.Value.CertificateThumbprint);
        }

        public byte[] BuildSignedCodeHash<T>(T hashModel, HashVersion hashVersion) where T : IHashSerializable
        {
            // implementation not for disclosure
            return Array.Empty<byte>();
        }

        public T DecodeSignedCodeHash<T>(byte[] hash, HashVersion hashVersion) where T : IHashDeserializable, new()
        {
            // implementation not for disclosure
            return default;
        }
    }
}