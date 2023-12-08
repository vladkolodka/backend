using FluentValidation;

namespace Menchul.MCode.Application.ProductUnits.Queries
{
    public class GetProductUnitByHashValidator : AbstractValidator<GetProductUnitByHashQuery>
    {
        public GetProductUnitByHashValidator()
        {
            RuleFor(h => h.EAN).NotEmpty();
            RuleFor(h => h.QR).NotEmpty();
        }
    }
}