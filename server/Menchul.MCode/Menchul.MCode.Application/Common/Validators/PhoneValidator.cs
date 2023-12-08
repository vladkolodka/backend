using FluentValidation;
using FluentValidation.Results;

namespace Menchul.MCode.Application.Common.Validators
{
    internal class PhoneValidator : AbstractValidator<string>
    {
        private int _digitsCount;
        private bool _invalidDigitFound;

        public PhoneValidator()
        {
            RuleFor(s => s).NotEmpty();

            RuleFor(s => s).Must(s => !_invalidDigitFound);
            RuleFor(s => s).Must(_ => _digitsCount is >= 4 and <= 15);
        }

        protected override bool PreValidate(ValidationContext<string> context, ValidationResult result)
        {
            if (string.IsNullOrEmpty(context.InstanceToValidate))
            {
                return base.PreValidate(context, result);
            }

            foreach (char c in context.InstanceToValidate)
            {
                switch (char.ToLowerInvariant(c))
                {
                    case ' ':
                    case '(':
                    case ')':
                    case '/':
                    case '\\':
                    case '-':
                    case '+':
                    case '.':
                    case 'e':
                    case 'x':
                    case 't':
                        continue;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        _digitsCount++;
                        continue;
                    default:
                        _invalidDigitFound = true;
                        break;
                }
            }

            return base.PreValidate(context, result);
        }
    }
}