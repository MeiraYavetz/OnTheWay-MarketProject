using AutoMapper;
using Common.DTOs;
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
    public class ProductStoreService : IGetService<ProductStoreDTO>
    {
        private readonly IGetRepository<ProductStore> _repository;

        private readonly IMapper _mapper;

        public ProductStoreService(IMapper mapper, IGetRepository<ProductStore> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<ProductStoreDTO>> GetAllAsync()
        {
            return _mapper.Map<List<ProductStoreDTO>>(await _repository.GetAllAsync());
        }
    }
}
