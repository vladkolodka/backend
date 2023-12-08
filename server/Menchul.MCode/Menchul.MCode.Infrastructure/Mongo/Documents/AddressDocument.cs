using Menchul.Core.ResourceProcessing.Entities;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Core.ValueObjects;
using Menchul.MCode.Infrastructure.Common;
using Menchul.Mongo.Resources;
using System.Collections.Generic;

namespace Menchul.MCode.Infrastructure.Mongo.Documents
{
    internal class AddressDocument : IDocumentResourceContainer
    {
        public AddressDocument(Address address)
        {
            CountryCode = address.Country?.TwoLetterISORegionName;
            PostCode = address.PostCode;
            RegionCode = address.RegionCode;
            AreaCode = address.AreaCode;
            Area = address.Area;
            Settlement = address.Settlement;
            Street = address.Street;
            Building = address.Building;
            Room = address.Room;
            Comments = address.Comments;
        }

        public string CountryCode { get; set; }
        public string PostCode { get; set; }
        public string RegionCode { get; set; }
        public string AreaCode { get; set; }
        public LocalizationRef Area { get; set; }
        public LocalizationRef Settlement { get; set; }
        public LocalizationRef Street { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        public LocalizationRef Comments { get; set; }

        public static AddressDocument NewOrNull(Address address) =>
            address == null ? null : new AddressDocument(address);

        public Address ToValueObject() =>
            new(CountryCode, PostCode, RegionCode, AreaCode, Area, Settlement, Street, Building, Room, Comments);

        public IEnumerable<IResource> GetResources()
        {
            yield return Area;
            yield return Settlement;
            yield return Street;
            yield return Comments;
        }

        public AddressDto ToDto() =>
            new()
            {
                Area = Area,
                Building = Building,
                Comments = Comments,
                Room = Room,
                Settlement = Settlement,
                Street = Street,
                AreaCode = AreaCode,
                CountryCode = CountryCode,
                PostCode = PostCode,
                RegionCode = RegionCode
            };
    }
}