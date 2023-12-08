using FluentValidation;
using Menchul.Application;
using Menchul.MCode.Application.Common.Dto;

namespace Menchul.MCode.Application.ProductUnits.Commands
{
    public class GenerateMQRCommandValidator : BaseCommandInfoValidator<GenerateMQRCommand, QrResultDto>
    {
        public GenerateMQRCommandValidator(IValidator<GenerateMQRCommand> validator) : base(validator)
        {
        }

        public class Validator : AbstractValidator<GenerateMQRCommand>
        {
            public Validator()
            {
                RuleFor(c => c.ProductId).NotEmpty().IdString12Bit();
                RuleFor(c => c.QRImageOptions).NotNull();
            }
        }
    }
}