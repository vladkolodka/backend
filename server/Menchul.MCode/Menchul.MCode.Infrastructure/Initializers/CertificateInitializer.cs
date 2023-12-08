using Convey.Types;
using Menchul.Certificate;
using Menchul.MCode.Infrastructure.Common.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Initializers
{
    public class CertificateInitializer : IInitializer
    {
        private readonly ICertificateManager _certificateManager;
        private readonly ILogger<CertificateInitializer> _logger;
        private readonly MenchulCertificateOptions _certificateOptions;

        public CertificateInitializer(ICertificateManager certificateManager,
            IOptions<MenchulCertificateOptions> certificateOptions, ILogger<CertificateInitializer> logger)
        {
            _certificateManager = certificateManager;
            _logger = logger;
            _certificateOptions = certificateOptions.Value;
        }

        public async Task InitializeAsync()
        {
            if (string.IsNullOrEmpty(_certificateOptions.PfxFile) || !File.Exists(_certificateOptions.PfxFile))
            {
                _logger.LogWarning("No certificate provided");
                return;
            }

            _logger.LogInformation("Initializing certificate...");

            string password = null;

            if (!string.IsNullOrEmpty(_certificateOptions.PasswordFile) &&
                File.Exists(_certificateOptions.PasswordFile))
            {
                password = await File.ReadAllTextAsync(_certificateOptions.PasswordFile);
            }

            if (string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("The password for menchul certificate is not found");
            }

            await _certificateManager.InstallPfxCertificateAsync(_certificateOptions.PfxFile, password?.Trim());
        }
    }
}