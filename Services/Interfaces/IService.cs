using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IService<T>
    {
        Task<T> GetByNameAsync(string name);
        Task<T> GetByIdAsync(int id);

        Task UpdateAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<List<T>> AddAsync(T entity);
        //Task<List<T>> AddAsync(StoreDetailsDTO entity);
        Task DeleteAsync(string name);
    }
}
