using FluentValidation;
using Menchul.MCode.Application.Common.Dto;

namespace Menchul.MCode.Application.Common.Validators
{
    public class AddressValidator : AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(a => a.CountryCode).NotEmpty().CultureCode();
            RuleFor(a => a.PostCode).NotEmpty().MaximumLength(10);
            RuleFor(a => a.RegionCode).NotEmpty().MaximumLength(5);
            RuleFor(a => a.AreaCode).MaximumLength(10);
            RuleFor(a => a.Area).LanguageValueCollection();
            RuleFor(a => a.Settlement).NotEmpty().LanguageValueCollection();
            RuleFor(a => a.Street).LanguageValueCollection();
            RuleFor(a => a.Building).NotEmpty().MaximumLength(10);
            RuleFor(a => a.Comments).LanguageValueCollection();
        }
    }
}