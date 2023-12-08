using Convey.CQRS.Commands;
using System;

namespace Menchul.MCode.Application.Companies.Commands.DeleteCompany
{
    public class DeleteCompanyCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}