using FluentValidation;
using Menchul.MCode.Application.Common.Validators;
using Menchul.MCode.Application.Products.Dto;
using Menchul.MCode.Core.Extensions;

namespace Menchul.MCode.Application.Products.Commands
{
    public class ProductCommandValidator : AbstractValidator<ProductCommandBase>
    {
        public ProductCommandValidator()
        {
            RuleFor(c => c.Name).LanguageValueCollection().NotEmpty();
            RuleFor(c => c.BrandName).LanguageValueCollection();
            RuleFor(c => c.Description).LanguageValueCollection();

            RuleFor(c => c.ManufacturerAddressOfManufacturing).SetValidator(new AddressValidator());
            RuleFor(c => c.BrandOwnerAddressOfManufacturing).SetValidator(new AddressValidator());

            RuleFor(c => c.DefaultLanguage).Must(s => s.IsValidIsoLanguageName())
                .WhenNotNull(c => c.DefaultLanguage);

            RuleFor(c => c.Metadata).SetValidator(new MetadataValidator());
        }
    }
}