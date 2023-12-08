using Menchul.Core.Entities;
using Menchul.Core.Types;
using Menchul.MCode.Core.Enums;
using Menchul.MCode.Core.Events;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Core.Entities
{
    public class Package : AggregateRoot<Id12Bytes>
    {
        public const int CurrentVersion = 0;

        public PackageState State { get; private set; }

        public IdGuid ClientCompanyId { get; }

        public DateTime DateOfPackaging { get; }

        public HashSet<Id12Bytes> Packages { get; }

        public HashSet<Id12Bytes> ProductUnits { get; }

        public string Properties { get; }

        public Package(Id12Bytes id, PackageState state, IdGuid clientCompanyId, DateTime? dateOfPackaging,
            HashSet<Id12Bytes> uniquePackageIds, HashSet<Id12Bytes> uniqueProductUnitIds, string properties,
            int version = CurrentVersion) : base(id, version)
        {
            if (!uniquePackageIds.Any() && !uniqueProductUnitIds.Any())
            {
                throw new PackageContentsInvalidException();
            }

            ClientCompanyId = clientCompanyId;
            State = state;
            DateOfPackaging = dateOfPackaging ?? DateTime.UtcNow;
            Packages = uniquePackageIds;
            ProductUnits = uniqueProductUnitIds;
            Properties = properties;
        }

        public static Package CreateNew(Id12Bytes id, IdGuid clientCompanyId, DateTime? dateOfPackaging,
            IEnumerable<IBsonId> packages, IEnumerable<IBsonId> productUnits, string properties)
        {
            var package = new Package(id, PackageState.Valid, clientCompanyId, dateOfPackaging, packages.ToIdsHashSet(),
                productUnits.ToIdsHashSet(), properties);

            package.AddEvent(new PackageCreatedEvent(package));

            return package;
        }

        public void Invalidate()
        {
            State = PackageState.Invalid;
        }
    }
}