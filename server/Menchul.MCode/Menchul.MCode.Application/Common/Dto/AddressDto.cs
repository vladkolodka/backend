using Menchul.MCode.Core.ValueObjects;

namespace Menchul.MCode.Application.Common.Dto
{
    public record AddressDto
    {
        public string CountryCode { get; set; }
        public string PostCode { get; set; }
        public string RegionCode { get; set; }
        public string AreaCode { get; set; }
        public LocalizedString Area { get; set; }
        public LocalizedString Settlement { get; set; }
        public LocalizedString Street { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        public LocalizedString Comments { get; set; }

        public Address ToValueObject()
        {
            return new(CountryCode, PostCode, RegionCode, AreaCode, Area, Settlement, Street, Building, Room, Comments);
        }
    }
}