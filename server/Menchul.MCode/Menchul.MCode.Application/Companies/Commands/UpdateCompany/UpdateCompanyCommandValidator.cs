using FluentValidation;
using Menchul.MCode.Application.Companies.Commands.CreateCompany;

namespace Menchul.MCode.Application.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            Include(new CreateCompanyCommandValidator.Validator());
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}