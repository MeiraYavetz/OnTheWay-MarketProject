using Microsoft.Extensions.DependencyInjection;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public static class ServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection service)
        {
            service.AddScoped<IRepository<Customer>, CustomerRepository>();
            service.AddScoped<IRepository<Store>, StoreRepository>();
            service.AddScoped<IRepository<Product>, ProductRepository>();
            service.AddScoped<IGetRepository<ProductStore>, ProductStoreRepository>();
            service.AddScoped<IExternalFunctionsService,StoreRepository>();
        }
    }
}
