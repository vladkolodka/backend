using FluentValidation;
using Menchul.Core.Helpers;
using Menchul.MCode.Application.Common.Validators;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.ValueObjects;
using System;
using System.Globalization;

namespace Menchul.MCode.Application
{
    internal static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> CultureCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Length(2)
                .Must(c => !ExceptionHelpers.ThrowsAny<Exception>(() => new RegionInfo(c)))
                .WithMessage("Region name is invalid.");
        }

        // TODO think about automation
        public static IRuleBuilderOptions<T, LocalizedString> LanguageValueCollection<T>(this IRuleBuilder<T, LocalizedString> ruleBuilder)
        {
            return ruleBuilder.SetValidator(LocalizedStringValidator.Instance.Value)
                .When(v => v is { }, ApplyConditionTo.CurrentValidator);
        }

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PhoneValidator());
        }

        public static IRuleBuilderOptions<T, string> UrlAddress<T>(this IRuleBuilder<T, string> ruleBuilder,
            UriKind kind = UriKind.RelativeOrAbsolute)
        {
            return ruleBuilder.Must(s => Uri.IsWellFormedUriString(s, kind))
                .WithMessage("The URL is invalid.");
        }

        public static IRuleBuilderOptions<T, string> WhenNotNull<T>(this IRuleBuilderOptions<T, string> ruleBuilder,
            Func<T, string> func)
        {
            return ruleBuilder.When(obj => !string.IsNullOrEmpty(func(obj)), ApplyConditionTo.CurrentValidator);
        }

        public static IRuleBuilderOptions<T, string> IdString12Bit<T>(this IRuleBuilder<T, string> ruleBuilder,
            bool canBeNull = false)
        {
            return ruleBuilder.Length(Id12Bytes.StringLength);
        }
    }
}