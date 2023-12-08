using Convey.CQRS.Queries;
using Convey.Types;
using FluentValidation;
using Menchul.Application;
using Menchul.Base.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.Infrastructure.Decorators
{
    [Decorator]
    public class ValidationQueryHandlerDecorator<TQuery, TResult> : ValidationDecoratorBase<TQuery>,
        IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _queryHandler;
        private readonly Guid _clientCompanyId;
        private readonly Guid _userId;

        public ValidationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> queryHandler,
            IEnumerable<IValidator<TQuery>> validators, ILogger<IQueryHandler<TQuery, TResult>> logger,
            IHttpContextAccessor contextAccessor, IAppContext appContext) : base(validators, contextAccessor, logger)
        {
            _queryHandler = queryHandler;
            _userId = appContext.Identity.Id;
            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            using (LogContext.PushProperty(LogProperties.UserId, _userId))
            using (LogContext.PushProperty(LogProperties.ClientCompanyId, _clientCompanyId))
            {
                await ValidateAsync(query);
                return await _queryHandler.HandleAsync(query);
            }
        }
    }
}