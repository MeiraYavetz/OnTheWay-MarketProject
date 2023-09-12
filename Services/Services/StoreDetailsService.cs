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
    public class StoreDetailsService:IServiceStore<StoreDTO>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Store> _repository;
        private readonly IExternalFunctionsService _external;
        public StoreDetailsService(IMapper mapper, IRepository<Store> repository, IExternalFunctionsService external)
        {
            _mapper = mapper;
            _repository = repository;
            _external = external;
        }
        public async Task<List<StoreDTO>> AddAsync(StoreDetailsDTO entity)
        {
            Store store = _mapper.Map<Store>(entity);
            await _repository.AddAsync(store);
            await _external.AddProductsToStoreAsync(store.StoreId, entity.Products);
            return _mapper.Map<List<StoreDTO>>(await _repository.GetAllAsync());
        }
    }
}
