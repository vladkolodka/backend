using Menchul.MCode.Core.Entities;
using System.Threading.Tasks;

namespace Menchul.MCode.Core.Repositories
{
    public interface IProductUnitRepository
    {
        Task AddAsync(ProductUnit unit);
    }
}