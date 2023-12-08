using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Models;
using Menchul.MCode.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Services
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly IHashManager _hashManager;
        private readonly IUrlManager _urlManager;
        private readonly IImageCreator _imageCreator;

        public CodeGenerator(IHashManager hashManager, IUrlManager urlManager, IImageCreator imageCreator)
        {
            _hashManager = hashManager;
            _urlManager = urlManager;
            _imageCreator = imageCreator;
        }

        public string CreateCode(QrUrlGenerationParameters parameters)
        {
            var signedHash = _hashManager.BuildSignedCodeHash(parameters.ToCodeId(), parameters.CodeType.HashVersion);
            return _urlManager.BuildQrInfoUrl(signedHash, parameters);
        }

        public async Task<QrResultDto> CreateCodeImage(QrImageGenerationParameters parameters)
        {
            var qrUrl = CreateCode(parameters);

            var image = await _imageCreator.CreateImageBase64Async(qrUrl, parameters.Options);

            return new QrResultDto {Id = parameters.Id, ImageBase64 = image};
        }
    }
}