using Convey.WebApi.Exceptions;
using Menchul.Application.Exceptions;
using Menchul.Core.Exceptions;
using Menchul.Infrastructure.Exceptions;
using Menchul.MCode.Core.Exceptions;
using Menchul.Mongo.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace Menchul.MCode.Infrastructure.Exceptions
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        private readonly IWebHostEnvironment _environment;
        private static readonly ConcurrentDictionary<Type, string> Codes = new();

        public ExceptionToResponseMapper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                ClientCompanyNotFoundException ex => DefaultExceptionResponse(ex, HttpStatusCode.PreconditionFailed),
                NotFoundException ex => DefaultExceptionResponse(ex, HttpStatusCode.NotFound),
                NotOwnedException ex => DefaultExceptionResponse(ex, HttpStatusCode.Forbidden),
                DomainException ex => DefaultExceptionResponse(ex, HttpStatusCode.BadRequest),

                AppValidationException ex => new ExceptionResponse(new {code = GetCode(ex), reason = ex.Message, errors = ex.Errors},
                    HttpStatusCode.BadRequest),

                EntityReferencedException ex => new ExceptionResponse(
                    new {code = GetCode(ex), reason = ex.Message, relations = ex.References},
                    HttpStatusCode.BadRequest),

                AppException ex => DefaultExceptionResponse(ex, HttpStatusCode.BadRequest),
                // TODO check logging
                Exception e => new ExceptionResponse(
                    new {code = "error", reason = _environment.IsDevelopment() ? e.Message + Environment.NewLine + e.StackTrace
                        : "There was an error."},
                    HttpStatusCode.InternalServerError)
            };

        private ExceptionResponse DefaultExceptionResponse(Exception ex, HttpStatusCode statusCode) =>
            new(new {code = GetCode(ex), reason = ex.Message}, statusCode);

        private static string GetCode(Exception exception)
        {
            var type = exception.GetType();
            if (Codes.TryGetValue(type, out var code))
            {
                return code;
            }

            var exceptionCode = exception.GetExceptionCode();
            Codes.TryAdd(type, exceptionCode);

            return exceptionCode;
        }
    }

}