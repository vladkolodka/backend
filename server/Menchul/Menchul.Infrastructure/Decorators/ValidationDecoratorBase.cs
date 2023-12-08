using FluentValidation;
using Menchul.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Menchul.Infrastructure.Decorators
{
    public abstract class ValidationDecoratorBase<T>
    {
        private readonly IEnumerable<IValidator<T>> _validators;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger _logger;

        protected ValidationDecoratorBase(IEnumerable<IValidator<T>> validators, IHttpContextAccessor contextAccessor, ILogger logger)
        {
            _validators = validators;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        protected async Task ValidateAsync(T request)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<T>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v =>
                    v.ValidateAsync(context,
                        _contextAccessor.HttpContext?.RequestAborted ?? default(CancellationToken))));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    throw new AppValidationException(failures);
                }
            }
        }

    }
}