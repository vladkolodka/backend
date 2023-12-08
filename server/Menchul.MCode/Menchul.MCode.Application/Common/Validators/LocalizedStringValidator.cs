using FluentValidation;
using Menchul.MCode.Core.Extensions;
using Menchul.MCode.Core.ValueObjects;
using System;

namespace Menchul.MCode.Application.Common.Validators
{
    public class LocalizedStringValidator : AbstractValidator<LocalizedString>
    {
        public static readonly Lazy<LocalizedStringValidator> Instance = new();

        public LocalizedStringValidator()
        {
            RuleFor(t => t.Translations).NotEmpty();

            RuleForEach(s => s.Translations)
                .Must(p => p.Key.IsValidIsoLanguageName())
                .Must(p => !string.IsNullOrEmpty(p.Value));
        }
    }
}