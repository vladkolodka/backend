using Convey.CQRS.Commands;
using Menchul.MCode.Application.Common.Dto;
using System;

namespace Menchul.MCode.Application.Companies.Commands.CreateClientCompany
{
    public record CreateClientCompanyCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public AddressDto LegalAddress { get; set; }
        public AddressDto MailAddress { get; set; }
    }
}