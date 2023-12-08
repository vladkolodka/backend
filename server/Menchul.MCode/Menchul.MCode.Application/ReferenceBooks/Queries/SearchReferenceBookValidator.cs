using FluentValidation;
using Menchul.MCode.Application.ReferenceBooks.Queries.Countries;
using Menchul.MCode.Application.ReferenceBooks.Queries.Currencies;

namespace Menchul.MCode.Application.ReferenceBooks.Queries
{
    public class SearchReferenceBookValidator : AbstractValidator<SearchReferenceBookQuery>
    {
        public SearchReferenceBookValidator()
        {
            RuleFor(q => q.Values).NotEmpty().When(q => !string.IsNullOrWhiteSpace(q.Key));
        }
    }

    public class SearchCountriesQueryValidator : AbstractValidator<SearchCountriesQuery>
    {
        public SearchCountriesQueryValidator()
        {
            Include(new SearchReferenceBookValidator());
        }
    }

    public class SearchCurrenciesQueryValidator : AbstractValidator<SearchCurrenciesQuery>
    {
        public SearchCurrenciesQueryValidator()
        {
            Include(new SearchReferenceBookValidator());
        }
    }
}