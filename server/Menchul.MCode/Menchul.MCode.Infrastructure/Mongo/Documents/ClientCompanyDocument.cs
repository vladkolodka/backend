using Menchul.Core.ResourceProcessing.Entities;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Infrastructure.Mongo.AggregateExtensions;
using Menchul.Mongo.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Infrastructure.Mongo.Documents
{
    internal class ClientCompanyDocument : IDocumentRoot<Guid>
    {
        public ClientCompanyDocument(ClientCompany clientCompany)
        {
            Id = clientCompany.Id.Value;
            InternalId = clientCompany.InternalId.ToObjectId();
            Name = clientCompany.Name;
            Phone = clientCompany.Phone;
            Email = clientCompany.Email;
            Url = clientCompany.Url;
            LegalAddress = new AddressDocument(clientCompany.LegalAddress);
            MailAddress = new AddressDocument(clientCompany.MailAddress);
            Version = clientCompany.Version;
        }

        public Guid Id { get; set; }

        public ObjectId InternalId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        public AddressDocument LegalAddress { get; set; }

        public AddressDocument MailAddress { get; set; }

        public int Version { get; set; }

        public ClientCompany ToEntity() =>
            new(Id, InternalId.ToAggregate(), Name, Email, Phone, Url, LegalAddress.ToValueObject(),
                MailAddress.ToValueObject(), Version);

        public IEnumerable<IResource> GetResources()
        {
            yield return LegalAddress;
            yield return MailAddress;
        }
    }
}