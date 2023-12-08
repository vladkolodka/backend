using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Entities;
using System.Collections.Generic;
using System.Globalization;

namespace Menchul.MCode.Core.ValueObjects
{
    public record Address : IResourceContainer
    {
        public RegionInfo Country { get; }
        public string PostCode { get; }
        public string RegionCode { get; }
        public string AreaCode { get; }
        public LocalizedString Area { get; }
        public LocalizedString Settlement { get; }
        public LocalizedString Street { get; }
        public string Building { get; }
        public string Room { get; }
        public LocalizedString Comments { get; }

        public Address(string countryCode, string postCode, string regionCode, string areaCode, LocalizedString area,
            LocalizedString settlement, LocalizedString street, string building, string room, LocalizedString comments)
        {
            try
            {
                Country = new RegionInfo(countryCode);
            }
            catch
            {
                // ignored
            }

            PostCode = postCode;
            RegionCode = regionCode;
            AreaCode = areaCode;
            Area = area;
            Settlement = settlement;
            Street = street;
            Building = building;
            Room = room;
            Comments = comments;
        }

        public void Accept(IResourceVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IEnumerable<IResource> GetResources()
        {
            yield return Area;
            yield return Settlement;
            yield return Street;
            yield return Comments;
        }
    }
}