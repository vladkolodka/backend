using Menchul.Application.Exceptions;
using System;

namespace Menchul.MCode.Application.Companies.Exceptions
{
    public class CompanyAlreadyExistsException : AppException
    {
        public Guid Id { get; }
        public string Email { get; }

        public CompanyAlreadyExistsException(Guid id, string email) : base(
            $"The company with the email '{email}' already exists.")
        {
            Id = id;
            Email = email;
        }

        public CompanyAlreadyExistsException(Guid id) : base(
            $"The company '{id}' already exists.")
        {
            Id = id;
        }
    }
}