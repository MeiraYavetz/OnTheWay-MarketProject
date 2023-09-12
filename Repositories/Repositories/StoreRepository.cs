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
    public class StoreRepository : IRepository<Store>,IExternalFunctionsService
    {
        private readonly IContext _context;
        public StoreRepository(IContext context)
        {
            _context = context;
        }
        public async Task<List<Store>> AddAsync(Store entity)
        {
            await _context.Stores.AddAsync(entity);
            await _context.SaveChangesAsync();
            return await _context.Stores.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var store = await _context.Stores.SingleAsync(e => e.StoreId == id);
            var productsToDelete = await _context.Products
            .Where(p => p.ProductStores.Count == 1 && p.ProductStores.Any(ps => ps.StoreId == id))
            .ToListAsync();
            var productStores = await _context.ProductStores.Where(ps => ps.StoreId == id).ToListAsync();
            _context.ProductStores.RemoveRange(productStores);
            _context.Stores.Remove(store);
            _context.Products.RemoveRange(productsToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Store>> GetAllAsync()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<Store> GetByNameAsync(string name)
        {
            return await _context.Stores.SingleAsync(e => e.StoreName == name);
        }
        public async Task<Store> GetByIdAsync(int id)
        {
            return await _context.Stores.SingleAsync(e => e.StoreId == id);
        }
        public async Task<Store> UpdateAsync(Store entity)
        {
            var c = _context.Stores.Update(entity);
            await _context.SaveChangesAsync();
            return c.Entity;
        }
        public async Task AddProductsToStoreAsync(int storeId, List<string> productNames)
        {
            foreach (string productName in productNames)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == productName);
                if (product == null)
                {
                    product = new Product { ProductName = productName };
                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();
                }
                ProductStore productStore = await _context.ProductStores
                    .FirstOrDefaultAsync(ps => ps.StoreId == storeId && ps.ProductId == product.ProductId);
                if (productStore == null)
                {
                    productStore = new ProductStore { StoreId = storeId, ProductId = product.ProductId };
                    await _context.ProductStores.AddAsync(productStore);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
