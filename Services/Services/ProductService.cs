using AutoMapper;
using Common.DTOs;
using Microsoft.EntityFrameworkCore.Migrations;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProductService : IService<ProductDTO>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _repository;
        public ProductService(IMapper mapper, IRepository<Product> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<ProductDTO>> AddAsync(ProductDTO entity)
        {
            var newOne = await _repository.AddAsync(_mapper.Map<Product>(entity));
            return _mapper.Map<List<ProductDTO>>(newOne);
        }

        public async Task DeleteAsync(string name)
        {
            var product=await _repository.GetByNameAsync(name);
            _repository.DeleteAsync(product.ProductId);
        }

        public async Task<List<ProductDTO>> GetAllAsync()
        {
            return _mapper.Map<List<ProductDTO>>(await _repository.GetAllAsync());


        }

        public async Task<ProductDTO> GetByNameAsync(string name)
        {
            return _mapper.Map<ProductDTO>(await _repository.GetByNameAsync(name));

        }
        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            return _mapper.Map<ProductDTO>(await _repository.GetByIdAsync(id));

        }
        public async Task UpdateAsync(ProductDTO entity)
        {
            await _repository.UpdateAsync(_mapper.Map<Product>(entity));
        }
    }
}
