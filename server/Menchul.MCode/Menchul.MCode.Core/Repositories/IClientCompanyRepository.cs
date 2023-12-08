using Menchul.Core.Entities;
using Menchul.MCode.Core.Entities;
using System.Threading.Tasks;

namespace Menchul.MCode.Core.Repositories
{
    public interface IClientCompanyRepository
    {
        Task<ClientCompany> GetAsync(IdGuid id);
        Task<ClientCompany> GetByEmailAsync(string email, bool loadLocalization = true);
        Task<bool> ExistsAsync(IdGuid id);
        Task AddAsync(ClientCompany clientCompany);
    }
}