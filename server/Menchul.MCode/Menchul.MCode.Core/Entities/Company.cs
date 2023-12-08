using Menchul.Core;
using Menchul.Core.Entities;
using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Entities;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Core.Entities
{
    public class Company :  AggregateRoot<IdGuid>, IAppResourceContainer, IClientCompanyOwned
    {
        public IdGuid ClientCompanyId { get; }
        public string Name { get; private set; }
        public LocalizedString Description { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Url { get; private set; }
        public Address Address { get; private set; }
        public IList<string> Certificates { get; private set; }

        public Company(Guid id, string name, LocalizedString description, string phone, string email, string url,
            Address address, IList<string> certificates, Guid clientCompanyId, int version = 0) : base(id, version)
        {
            Rename(name);
            ChangeDescription(description);
            ChangePhone(phone);
            ChangeEmail(email);
            ChangeUrl(url);
            ChangeAddress(address);
            ReplaceCertificates(certificates);
            ClientCompanyId = clientCompanyId;
        }

        public void Accept(IResourceVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IEnumerable<IResource> GetResources()
        {
            yield return Description;
            yield return Address;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new InvalidCompanyDetailException(nameof(Name), newName);
            }

            Name = newName;
        }

        public void ChangeDescription(LocalizedString newDescription)
        {
            Description = newDescription;
        }

        public void ChangePhone(string newPhone)
        {
            if (newPhone.IsWhiteSpace())
            {
                throw new InvalidCompanyDetailException(nameof(Phone), newPhone);
            }

            Phone = newPhone;
        }

        public void ChangeEmail(string newEmail)
        {
            if (newEmail.IsWhiteSpace())
            {
                throw new InvalidCompanyDetailException(nameof(Email), newEmail);
            }

            Email = newEmail;
        }

        public void ChangeUrl(string newUrl)
        {
            if (!string.IsNullOrEmpty(newUrl) && !Uri.IsWellFormedUriString(newUrl, UriKind.RelativeOrAbsolute))
            {
                throw new InvalidCompanyUrlException(newUrl);
            }

            Url = newUrl;
        }

        public void ChangeAddress(Address newAddress)
        {
            Address = newAddress;
        }

        public void ReplaceCertificates(IList<string> newCertificates)
        {
            Certificates = newCertificates;
        }

        IdGuid IClientCompanyOwned.GetCompanyId() => ClientCompanyId;
    }
}