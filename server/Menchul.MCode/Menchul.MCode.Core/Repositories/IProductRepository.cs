using Menchul.MCode.Core.Entities;
using System.Threading.Tasks;

namespace Menchul.MCode.Core.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(ProductInformation product);
        Task<Product> GetAsync(Id12Bytes id);
        Task<ProductInformation> GetInformationAsync(Id12Bytes id);
        Task<bool> EANExistsAsync(long ean);
        Task UpdateAsync(ProductInformation product);
        Task DeleteAsync(Id12Bytes id);
    }
}