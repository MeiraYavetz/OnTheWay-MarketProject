using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetByNameAsync(string name);
        Task<T> GetByIdAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<List<T>> AddAsync(T entity);
        Task DeleteAsync(int id);
    }
}
