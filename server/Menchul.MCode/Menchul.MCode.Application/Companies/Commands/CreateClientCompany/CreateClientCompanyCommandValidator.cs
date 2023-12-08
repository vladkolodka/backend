using FluentValidation;
using Menchul.Application;
using Menchul.MCode.Application.Common.Dto;
using System;

namespace Menchul.MCode.Application.Companies.Commands.CreateClientCompany
{
    public class CreateClientCompanyCommandValidator : BaseCommandInfoValidator<CreateClientCompanyCommand, Guid>
    {
        public CreateClientCompanyCommandValidator(IValidator<CreateClientCompanyCommand> validator) : base(validator)
        {
        }

        public class Validator : AbstractValidator<CreateClientCompanyCommand>
        {
            public Validator(IValidator<AddressDto> addressValidator)
            {
                RuleFor(c => c.Name).NotEmpty();
                RuleFor(c => c.Email).NotEmpty().EmailAddress();

                RuleFor(c => c.LegalAddress).NotNull().SetValidator(addressValidator);
                RuleFor(c => c.MailAddress).NotNull().SetValidator(addressValidator);
            }
        }
    }
}