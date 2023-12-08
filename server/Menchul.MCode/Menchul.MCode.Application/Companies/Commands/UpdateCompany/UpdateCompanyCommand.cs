using Convey.CQRS.Commands;
using Menchul.MCode.Application.Companies.Commands.CreateCompany;
using System;

namespace Menchul.MCode.Application.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommand : CreateCompanyCommand, ICommand
    {
        public Guid Id { get; set; }
    }
}