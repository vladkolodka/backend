using Convey.CQRS.Queries;
using Menchul.Resources.ReferenceBooks.Models;
using System.Collections.Generic;

namespace Menchul.MCode.Application.ReferenceBooks.Queries.Currencies
{
    public class SearchCurrenciesQuery : SearchReferenceBookQuery, IQuery<List<CurrencyModel>>
    {
    }
}