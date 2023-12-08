using FluentValidation;
using Menchul.Application;
using Menchul.MCode.Application.Common.Dto;

namespace Menchul.MCode.Application.Packages.Commands
{
    public class GeneratePQRCommandValidator : BaseCommandInfoValidator<GeneratePQRCommand, QrResultDto>
    {
        public GeneratePQRCommandValidator(IValidator<GeneratePQRCommand> validator) : base(validator)
        {
        }

        public class Validator : AbstractValidator<GeneratePQRCommand>
        {
            public Validator()
            {
                RuleForEach(c => c.Packages).NotEmpty().IdString12Bit();
                RuleForEach(c => c.Products).NotEmpty().IdString12Bit();
                RuleFor(c => c.QRImageOptions).NotNull();
            }
        }
    }
}