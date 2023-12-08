using FluentValidation;
using Menchul.MCode.Application.Common.Validators;
using Menchul.MCode.Application.Companies.Commands.CreateCompany;

namespace Menchul.MCode.Application.Companies.Validators
{
    public class BaseCompanyValidator<T> : AbstractValidator<T> where T : CreateCompanyCommand
    {
        protected BaseCompanyValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Description).LanguageValueCollection();
            RuleFor(c => c.Phone).PhoneNumber().WhenNotNull(c => c.Phone);
            RuleFor(c => c.Email).EmailAddress().WhenNotNull(c => c.Email);
            RuleFor(c => c.Url).UrlAddress().WhenNotNull(c => c.Url);

            RuleFor(c => c.Address).SetValidator(new AddressValidator());

            RuleFor(c => c.Certificates).NotEmpty().When(c => c.Certificates != null);
        }
    }
}