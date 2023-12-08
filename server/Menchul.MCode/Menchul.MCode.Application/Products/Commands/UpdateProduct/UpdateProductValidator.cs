using FluentValidation;

namespace Menchul.MCode.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            Include(new ProductCommandValidator());
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}