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
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly IContext _context;
        public CustomerRepository(IContext context)
        {
            _context = context;
        }
        public async Task<List<Customer>> AddAsync(Customer entity)
        {
            await _context.Customers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return await _context.Customers.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var c = await GetByIdAsync(id);
            _context.Customers.Remove(c);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByNameAsync(string name)
        {
            return await _context.Customers.SingleAsync(e => e.CustomerName == name);
        }
        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.SingleAsync(e => e.CustomerId == id);
        }
        public async Task<Customer> UpdateAsync(Customer entity)
        {
            var c = _context.Customers.Update(entity);
            await _context.SaveChangesAsync();
            return c.Entity;
        }
    }
}
