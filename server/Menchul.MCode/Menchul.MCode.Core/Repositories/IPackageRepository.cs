using Menchul.MCode.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Core.Repositories
{
    public interface IPackageRepository
    {
        Task AddAsync(Package package);
        Task<List<Package>> GetConflictingAsync(Package package);
        Task UpdateStateAllAsync(List<Package> packages);
    }
}