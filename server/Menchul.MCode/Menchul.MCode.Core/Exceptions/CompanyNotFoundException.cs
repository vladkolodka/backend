using System;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Core.Exceptions
{
    public class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException() : base($"The company wasn't found.")
        {
        }

        private CompanyNotFoundException(string ids) : base($"The company {ids} wasn't found.")
        {
        }

        public CompanyNotFoundException(Guid companyId) : this($"[{companyId}]")
        {
        }

        public CompanyNotFoundException(IEnumerable<Guid> companyId) : this(string.Join(", ",
            companyId.Select(id => $"[{id}]")))
        {
        }

        public CompanyNotFoundException(IEnumerable<Guid> allIds, IEnumerable<Guid> foundIds) : this(allIds.Except(foundIds))
        {
        }
    }
}