using Convey.CQRS.Queries;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Exceptions;
using Menchul.MCode.Application.Common.Models;
using Menchul.MCode.Application.ProductUnits.Exceptions;
using Menchul.MCode.Application.ProductUnits.Queries;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Entities;
using Menchul.Mongo;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.ProductUnits
{
    internal class GetProductUnitByHashQueryHandler : GetProductUnitQueryHandlerBase, IQueryHandler<GetProductUnitByHashQuery, PassportDto>
    {
        private readonly IUrlManager _urlManager;
        private readonly IHashManager _hashManager;

        public GetProductUnitByHashQueryHandler(IRelationManager relationManager, IUrlManager urlManager, IHashManager hashManager) : base(relationManager)
        {
            _urlManager = urlManager;
            _hashManager = hashManager;
        }

        public async Task<PassportDto> HandleAsync(GetProductUnitByHashQuery query)
        {
            var hash = _urlManager.DecodeQr(query.QR);
            var codeId = _hashManager.DecodeSignedCodeHash<CodeId>(hash, query.CodeType.HashVersion);

            var unitId = new ObjectId(codeId.Id);

            var unitDocument = await GetProductUnitById(unitId, query);

            if (unitDocument.Version != ProductUnit.CurrentVersion)
            {
                throw new CodeVersionMismatch(ProductUnit.CurrentVersion, unitDocument.Version);
            }

            if (unitDocument.Product?.Document.EAN != query.EAN)
            {
                throw new EanInvalid();
            }

            return PassportFrom(unitDocument);
        }
    }
}