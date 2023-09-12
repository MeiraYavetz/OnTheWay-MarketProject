
using Common.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using Services;
using Services.Algorithm;

namespace Services
{
    public static class ServiceCollectionExtension
    {
        public static void AddService(this IServiceCollection services)
        {
            services.AddScoped<IServiceStore<StoreDTO>,StoreDetailsService>();
            services.AddScoped<IService<CustomerDTO>, CustomerService>();
            services.AddScoped<IService<ProductDTO>, ProductService>();
            services.AddScoped<IService<StoreDTO>, StoreService>();
            services.AddScoped < IGetService<ProductStoreDTO>, ProductStoreService>();
            services.AddScoped<IAlgorithm, MainAlgorithm>();
            services.AddAutoMapper(typeof(Mapper));
            services.AddRepositories();
        }
    }
}
