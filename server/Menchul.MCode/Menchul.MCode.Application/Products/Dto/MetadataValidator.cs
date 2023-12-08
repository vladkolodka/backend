using FluentValidation;
using Menchul.MCode.Application.Products.DescriptorValidators;

namespace Menchul.MCode.Application.Products.Dto
{
    public class MetadataValidator : AbstractValidator<MetadataDocumentDto>
    {
        public MetadataValidator()
        {
            RuleFor(c => c.Descriptors).NotEmpty();

            When(c => c.Version == DescriptorV01Validator.Version, delegate
            {
                var descriptorV01Validator = new DescriptorV01Validator();

                RuleForEach(c => c.Descriptors)
                    .SetValidator(descriptorV01Validator);
            });
        }
    }
}