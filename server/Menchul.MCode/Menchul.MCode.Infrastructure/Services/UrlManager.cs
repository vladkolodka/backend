using Menchul.MCode.Application.Common.Configuration;
using Menchul.MCode.Application.Common.Exceptions;
using Menchul.MCode.Application.Common.Models;
using Menchul.MCode.Application.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartFormat;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Infrastructure.Services
{
    public class UrlManager : IUrlManager
    {
        private readonly ILogger<UrlManager> _logger;
        private readonly QrGenerationOptions _qrOptions;

        public UrlManager(IOptions<QrGenerationOptions> qrOptions, ILogger<UrlManager> logger)
        {
            _logger = logger;
            _qrOptions = qrOptions.Value;
        }

        public string BuildQrInfoUrl(byte[] data, QrUrlGenerationParameters parameters)
        {
            var dataString = Base64UrlTextEncoder.Encode(data);

            var payload = string.IsNullOrWhiteSpace(parameters.UrlPayloadFormat)
                ? string.Empty
                : parameters.UrlPayloadContent == null
                    ? parameters.UrlPayloadFormat
                    : Smart.Format(parameters.UrlPayloadFormat, parameters.UrlPayloadContent);

            if (!string.IsNullOrEmpty(payload))
            {
                payload = "/" + payload;
            }

            var url = $"{_qrOptions.BaseUrl}/{parameters.CodeType.Path}{payload}";

            var queryString = new Dictionary<string, string> {{_qrOptions.QueryParameterName, dataString}};

            var builtUrl = QueryHelpers.AddQueryString(url, queryString);

            _logger.LogDebug("Generated QR URL {Url}", builtUrl);

            return builtUrl;
        }

        public byte[] DecodeQr(string qr)
        {
            try
            {
                return Base64UrlTextEncoder.Decode(qr);
            }
            catch (FormatException)
            {
                throw new InvalidCodeSignatureException();
            }
        }
    }
}