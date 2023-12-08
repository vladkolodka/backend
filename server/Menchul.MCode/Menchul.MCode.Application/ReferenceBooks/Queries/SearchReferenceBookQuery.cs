using System.Collections.Generic;

namespace Menchul.MCode.Application.ReferenceBooks.Queries
{
    public class SearchReferenceBookQuery
    {
        public string Key { get; set; }
        public IList<dynamic> Values { get; set; }
    }
}