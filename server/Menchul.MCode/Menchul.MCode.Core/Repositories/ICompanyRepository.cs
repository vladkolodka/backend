using Menchul.Core.Entities;
using Menchul.MCode.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Core.Repositories
{
    public interface ICompanyRepository
    {
        Task AddAsync(Company company);

        Task UpdateAsync(Company company);

        Task<Company> GetAsync(IdGuid id);
        Task DeleteAsync(IdGuid id);

        Task<List<Guid>> ExistsAllAsync(List<Guid> ids);
    }
}