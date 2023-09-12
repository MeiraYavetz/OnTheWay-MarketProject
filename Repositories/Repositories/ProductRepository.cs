using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly IContext _context;
        public ProductRepository(IContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> AddAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
            return await _context.Products.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var c = await GetByIdAsync(id);
            _context.Products.Remove(c);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _context.Products.SingleAsync(e => e.ProductName == name);
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.SingleAsync(e => e.ProductId == id);
        }
        public async Task<Product> UpdateAsync(Product entity)
        {
            var c = _context.Products.Update(entity);
            await _context.SaveChangesAsync();
            return c.Entity;
        }
    }
}
