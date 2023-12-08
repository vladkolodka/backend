using Menchul.Core.Entities;
using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Entities;
using Menchul.MCode.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Core.Entities
{
    public class ClientCompany : AggregateRoot<IdGuid>, IAppResourceContainer
    {
        public Id12Bytes InternalId { get; }
        public string Name { get; }

        public string Phone { get; }

        public string Email { get; }

        public string Url { get; }

        public Address LegalAddress { get; }

        public Address MailAddress { get; }

        public ClientCompany(Guid id, Id12Bytes internalId, string name, string email, string phone, string url,
            Address legalAddress, Address mailAddress, int version = 0) : base(id, version)
        {
            InternalId = internalId;
            Name = name;
            Phone = phone;
            Email = email;
            Url = url;
            LegalAddress = legalAddress;
            MailAddress = mailAddress;
        }

        public void Accept(IResourceVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IEnumerable<IResource> GetResources()
        {
            yield return LegalAddress;
            yield return MailAddress;
        }
    }
}