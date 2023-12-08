using Convey.CQRS.Queries;
using Menchul.Core.Entities;
using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Application.Common.Exceptions;
using Menchul.MCode.Application.Common.Models;
using Menchul.MCode.Application.Packages.Dto;
using Menchul.MCode.Application.Packages.Queries;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Repositories;
using Menchul.Mongo;
using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.Packages
{
    internal class GetPackageByHashQueryHandler : GetPackageQueryHandlerBase,
        IQueryHandler<GetPackageByHashQuery, PackageDto>
    {
        private readonly IClientCompanyRepository _companyRepository;
        private readonly IUrlManager _urlManager;
        private readonly IHashManager _hashManager;

        public GetPackageByHashQueryHandler(IRelationManager relationManager,
            IClientCompanyRepository companyRepository,
            IUrlManager urlManager, IHashManager hashManager) : base(relationManager)
        {
            _companyRepository = companyRepository;
            _urlManager = urlManager;
            _hashManager = hashManager;
        }

        public async Task<PackageDto> HandleAsync(GetPackageByHashQuery query)
        {
            var hash = _urlManager.DecodeQr(query.QR);
            var codeId = _hashManager.DecodeSignedCodeHash<CodeId>(hash, QrCodeType.PackageV1.HashVersion);

            var packageId = new ObjectId(codeId.Id);

            var packageDocument = await GetPackageById(packageId, query);

            if (packageDocument == null)
            {
                return null;
            }

            if (packageDocument.Version != Package.CurrentVersion)
            {
                throw new CodeVersionMismatch(Package.CurrentVersion, packageDocument.Version);
            }


            var company = await _companyRepository.GetAsync(new IdGuid(packageDocument.ClientCompany));

            if (company == null || !codeId.ParentId.SequenceEqual(company.InternalId.Value.ToByteArray()))
            {
                throw new InvalidCodeSignatureException();
            }

            return packageDocument.ToDto();
        }
    }
}