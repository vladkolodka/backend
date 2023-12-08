using Convey.CQRS.Commands;
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
    public class ValidationCommandHandlerDecorator<TCommand> : ValidationDecoratorBase<TCommand>,
        ICommandHandler<TCommand> where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly Guid _clientCompanyId;
        private readonly Guid _userId;

        public ValidationCommandHandlerDecorator(ICommandHandler<TCommand> handler,
            IEnumerable<IValidator<TCommand>> validators, ILogger<ICommandHandler<TCommand>> logger,
            IHttpContextAccessor contextAccessor, IAppContext appContext) : base(validators, contextAccessor, logger)
        {
            _handler = handler;
            _userId = appContext.Identity.Id;
            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        public async Task HandleAsync(TCommand command)
        {
            using (LogContext.PushProperty(LogProperties.UserId, _userId))
            using (LogContext.PushProperty(LogProperties.ClientCompanyId, _clientCompanyId))
            {
                await ValidateAsync(command);
                await _handler.HandleAsync(command);
            }
        }
    }
}