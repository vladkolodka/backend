using Convey.CQRS.Queries;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.Resources.ReferenceBooks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.ReferenceBooks.Queries.Currencies
{
    public class SearchCurrenciesQueryHandler : SearchReferenceBookHandlerBase<CurrencyModel>,
        IQueryHandler<SearchCurrenciesQuery, List<CurrencyModel>>
    {
        public SearchCurrenciesQueryHandler(IReferenceBookCache referenceBookCache) : base(referenceBookCache,
            new[] {nameof(CurrencyModel.Alpha3), nameof(CurrencyModel.Code)})
        {
        }

        public Task<List<CurrencyModel>> HandleAsync(SearchCurrenciesQuery query)
        {
            return LoadAsync(query);
        }
    }
}