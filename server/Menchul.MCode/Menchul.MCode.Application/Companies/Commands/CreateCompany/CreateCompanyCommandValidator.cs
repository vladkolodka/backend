using FluentValidation;
using Menchul.Application;
using System;

namespace Menchul.MCode.Application.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommandValidator : BaseCommandInfoValidator<CreateCompanyCommand, Guid>
    {
        public CreateCompanyCommandValidator(IValidator<CreateCompanyCommand> validator) : base(validator)
        {
        }

        public class Validator : AbstractValidator<CreateCompanyCommand>
        {
        }
    }
}