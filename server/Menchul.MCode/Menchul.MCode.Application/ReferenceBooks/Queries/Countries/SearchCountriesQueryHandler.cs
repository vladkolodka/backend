using Convey.CQRS.Queries;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.Resources.ReferenceBooks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.ReferenceBooks.Queries.Countries
{
    public class SearchCountriesQueryHandler : SearchReferenceBookHandlerBase<CountryModel>,
        IQueryHandler<SearchCountriesQuery, List<CountryModel>>
    {
        public SearchCountriesQueryHandler(IReferenceBookCache referenceBookCache) : base(referenceBookCache,
            new[] {nameof(CountryModel.Alpha2), nameof(CountryModel.Alpha3), nameof(CountryModel.Code)})
        {
        }

        public Task<List<CountryModel>> HandleAsync(SearchCountriesQuery query)
        {
            return LoadAsync(query);
        }
    }
}