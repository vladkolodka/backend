using FluentValidation;
using Menchul.Application;
using System;

namespace Menchul.MCode.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : BaseCommandInfoValidator<CreateProductCommand, string>
    {
        public CreateProductCommandValidator(IValidator<CreateProductCommand> validator) : base(validator)
        {
        }

        public class Validator : AbstractValidator<CreateProductCommand>
        {
            public Validator()
            {
                Include(new ProductCommandValidator());

                RuleFor(c => c.ParentProductId).IdString12Bit().WhenNotNull(c => c.ParentProductId);
                RuleFor(c => c.EAN).NotEmpty();

                RuleForEach(c => c.Urls)
                    .NotEmpty()
                    .Must(u => !string.IsNullOrEmpty(u?.Address) && Uri.TryCreate(u.Address, UriKind.Absolute, out _));
            }
        }
    }
}