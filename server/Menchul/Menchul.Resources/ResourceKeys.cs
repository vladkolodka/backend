using Menchul.Resources.ReferenceBooks.Models;
using System;
using System.Collections.Generic;

namespace Menchul.Resources
{
    /// <summary>
    /// The values in this class should match values in the <see cref="ReferenceBooksResource"/>.
    /// You need to update this class manually after renaming the file.
    /// </summary>
    public class ResourceKeys
    {
        public static Dictionary<Type, string> ReferenceBooks = new()
        {
            {typeof(CountryModel), "countries"},
            {typeof(CurrencyModel), "currencies"}
        };
    }
}