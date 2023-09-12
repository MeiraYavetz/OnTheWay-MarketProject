using AutoMapper;
using Common.DTOs;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Store,StoreDetailsDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Store, StoreDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductStore, ProductStoreDTO>().ReverseMap();
        }
    }
}
