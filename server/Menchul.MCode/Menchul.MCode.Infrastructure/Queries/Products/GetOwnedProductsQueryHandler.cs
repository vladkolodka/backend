using Convey.CQRS.Queries;
using Menchul.Application;
using Menchul.MCode.Application.Products.Dto;
using Menchul.MCode.Application.Products.Queries;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.Products
{
    internal class GetOwnedProductsQueryHandler : IQueryHandler<GetOwnedProductsQuery, List<ProductDto>>
    {
        private readonly IRelationManager _manager;
        private readonly IRelationManager _relationManager;
        private readonly Guid _clientCompanyId;

        public GetOwnedProductsQueryHandler(IAppContext appContext, IRelationManager manager, IRelationManager relationManager)
        {
            _manager = manager;
            _relationManager = relationManager;
            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        public async Task<List<ProductDto>> HandleAsync(GetOwnedProductsQuery query)
        {
            var products =
                await _manager.Load<ProductDocument, ObjectId>(f =>
                    f.Match(p => p.ClientCompany.RefId == _clientCompanyId));

            await _relationManager.LoadLocalization(products, query);

            return products.Select(d => d.ToDto()).ToList();
        }
    }
}