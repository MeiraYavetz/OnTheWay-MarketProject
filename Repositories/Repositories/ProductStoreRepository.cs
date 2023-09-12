using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ProductStoreRepository : IGetRepository<ProductStore>
    {
        private readonly IContext _context;
        public ProductStoreRepository(IContext context)
        {
            _context = context;
        }
        public async Task<List<ProductStore>> GetAllAsync()
        {
            return await _context.ProductStores.ToListAsync();
        }
    }
}
