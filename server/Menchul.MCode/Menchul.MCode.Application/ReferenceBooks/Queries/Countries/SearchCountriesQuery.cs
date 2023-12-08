using Convey.CQRS.Queries;
using Menchul.Resources.ReferenceBooks.Models;
using System.Collections.Generic;

namespace Menchul.MCode.Application.ReferenceBooks.Queries.Countries
{
    public class SearchCountriesQuery : SearchReferenceBookQuery, IQuery<List<CountryModel>>
    {
    }
}