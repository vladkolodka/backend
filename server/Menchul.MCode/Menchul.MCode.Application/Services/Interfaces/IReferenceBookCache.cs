using Menchul.Resources.ReferenceBooks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IReferenceBookCache
    {
        public Task<List<T>> GetAllAsync<T>() where T : IReferenceBookModel;
    }
}