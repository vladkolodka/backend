using Menchul.Core.ResourceProcessing.Entities;
using Menchul.MCode.Application.Companies.Dto;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Infrastructure.Common;
using Menchul.Mongo.Common;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Infrastructure.Mongo.Documents
{
    internal class CompanyDocument: IDocumentRoot<Guid>
    {
        // TODO why GUID?
        public Guid Id { get; set; }

        public CompanyDocument(Company company)
        {
            Id = company.Id;
            Name = company.Name;
            Description = company.Description;
            Phone = company.Phone;
            Email = company.Email;
            Url = company.Url;
            Address = new AddressDocument(company.Address);
            Certificates = company.Certificates;
            ClientCompany = company.ClientCompanyId.Value;
            Version = company.Version;
        }

        public string Name { get; set; }
        public LocalizationRef Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public AddressDocument Address { get; set; }
        public IList<string> Certificates { get; set; }

        public DocumentRef<ClientCompanyDocument, Guid> ClientCompany { get; set; }
        public int Version { get; set; }

        public Company ToEntity() =>
            new(Id, Name, Description, Phone, Email, Url, Address.ToValueObject(), Certificates, ClientCompany, Version);

        public IEnumerable<IResource> GetResources()
        {
            yield return ClientCompany;
            yield return Description;
            yield return Address;
        }

        public CompanyDto ToDto() =>
            new()
            {
                Name = Name,
                Description = Description,
                Address = Address.ToDto(),
                Email = Email,
                Certificates = Certificates,
                Phone = Phone,
                Url = Url
            };

    }
}