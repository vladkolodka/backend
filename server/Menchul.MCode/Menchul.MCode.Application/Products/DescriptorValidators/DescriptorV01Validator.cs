using FluentValidation;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Products.DescriptorValidators
{
    public class DescriptorV01Validator : AbstractValidator<JToken>
    {
        public const string Version = "0.1";
        private const string _idProperty = "id";

        private readonly HashSet<string> _validProperties = new()
        {
            _idProperty, "component", "binding", "parentId"
        };

        public DescriptorV01Validator()
        {
            Transform(t => t, o => o as JObject).Must(j => j.ContainsKey(_idProperty))
                .WithMessage($"The descriptor should have an [{_idProperty}] property");

            // TODO transform for collection rules doesn't supported yet (at least in current version used)
            RuleForEach(t => t)
                .Must(t => _validProperties.Contains((t as JProperty)?.Name))
                .WithMessage("Unknown descriptor property");
        }
    }
}