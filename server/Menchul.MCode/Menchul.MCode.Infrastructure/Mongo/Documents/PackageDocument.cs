using Menchul.Core;
using Menchul.Core.ResourceProcessing.Entities;
using Menchul.Core.Types;
using Menchul.MCode.Application.Packages.Dto;
using Menchul.MCode.Application.ProductUnits.Dto;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Enums;
using Menchul.MCode.Core.Extensions;
using Menchul.MCode.Infrastructure.Mongo.AggregateExtensions;
using Menchul.MCode.Infrastructure.Types;
using Menchul.Mongo.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Infrastructure.Mongo.Documents
{
    internal class PackageDocument : IDocumentRoot<ObjectId>, IIdMutable<Id12Bytes, IBsonId>
    {
        public static readonly string PackagesName = nameof(Packages).ToCamelCase();
        public static readonly string ProductUnitsName = nameof(ProductUnits).ToCamelCase();

        public PackageDocument(Package package)
        {
            SetId(package.Id);
            State = package.State;
            ClientCompany = package.ClientCompanyId.Value;
            DateOfPackaging = package.DateOfPackaging;

            Packages = package.Packages.Select(id => id.ToObjectId()).ToList();
            ProductUnits = package.ProductUnits.Select(id => id.ToObjectId()).ToList();

            Properties = package.Properties;
            Version = package.Version;
        }

        public PackageDocument()
        {
        }

        public virtual IEnumerable<IResource> GetResources()
        {
            yield return ClientCompany;
            yield return ProductUnits;
            yield return Packages;
        }

        public ObjectId Id { get; set; }

        public PackageState State { get; set; }

        public DocumentRef<ClientCompanyDocument, Guid> ClientCompany { get; set; }

        public DateTime DateOfPackaging { get; set; }
        public DocumentReferenceList<ProductUnitDocument, ObjectId> ProductUnits { get; set; }
        public DocumentReferenceList<PackageDocument, ObjectId> Packages { get; set; }
        public string Properties { get; set; }
        public int Version { get; set; }

        public Package ToEntity()
        {
            var packageIds = Packages?.Select(r => new BsonId(r.RefId));
            var unitIds = ProductUnits?.Select(r => new BsonId(r.RefId));

            return new Package(Id.ToAggregate(), State, ClientCompany.RefId, DateOfPackaging, packageIds.ToIdsHashSet(),
                unitIds.ToIdsHashSet(), Properties, Version);
        }

        public PackageDto ToDto()
        {
            return new()
            {
                Id = Id.ToString(),
                State = State,
                DateOfPackaging = DateOfPackaging,
                Packages = Packages?.Select(r => r.Document?.ToDto() ?? new PackageDto { Id = r.RefId.ToString() })
                    .ToList(),
                Products = ProductUnits
                    ?.Select(r => r.Document?.ToDto() ?? new ProductUnitDto { Id = r.RefId.ToString() })
                    .ToList(),
                PropertiesRaw = Properties
            };
        }

        private sealed class PackageIdEqualityComparer : IEqualityComparer<PackageDocument>
        {
            public bool Equals(PackageDocument x, PackageDocument y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null))
                {
                    return false;
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return x.Id.Equals(y.Id);
            }

            public int GetHashCode(PackageDocument obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        public static IEqualityComparer<PackageDocument> IdComparer { get; } = new PackageIdEqualityComparer();

        public void SetId(Id12Bytes aggregate)
        {
            Id = aggregate.ToObjectId();
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class PackageDocumentWithHierarchy : PackageDocument, IHierarchy<PackageDocument>
    {
        public static readonly string HierarchyName = nameof(Hierarchy).ToCamelCase();
        public List<PackageDocument> Hierarchy { get; set; }

        public override IEnumerable<IResource> GetResources() => base.GetResources().Concat(Hierarchy);
    }
}