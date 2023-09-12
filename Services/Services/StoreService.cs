using AutoMapper;
using Repositories.Interfaces;
using Common.DTOs;
using Repositories.Entities;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Execution;
using Repositories.Repositories;
using System.Security.Cryptography.X509Certificates;

namespace Services.Services
{
    public class StoreService : IService<StoreDTO>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Store> _repository;
        private readonly IExternalFunctionsService _external;
        public StoreService(IMapper mapper, IRepository<Store> repository, IExternalFunctionsService external)
        {
            _mapper = mapper;
            _repository = repository;
            _external = external;
        }

        public async Task<List<StoreDTO>> AddAsync(StoreDTO entity)
        {
            var c = await _repository.AddAsync(_mapper.Map<Store>(entity));
            return _mapper.Map<List<StoreDTO>>(c);
        }
       
        public async Task DeleteAsync(string name)
        {
            var store = await _repository.GetByNameAsync(name);
            await _repository.DeleteAsync(store.StoreId);
        }
        


        public async Task<List<StoreDTO>> GetAllAsync()
        {
            return _mapper.Map<List<StoreDTO>>(await _repository.GetAllAsync());


        }

        public async Task<StoreDTO> GetByNameAsync(string name)
        {
            return _mapper.Map<StoreDTO>(await _repository.GetByNameAsync(name));

        }
        public async Task<StoreDTO> GetByIdAsync(int id)
        {
            return _mapper.Map<StoreDTO>(await _repository.GetByIdAsync(id));

        }
        public async Task UpdateAsync(StoreDTO entity)
        {
            await _repository.UpdateAsync(_mapper.Map<Store>(entity));
        }

       

    }
}
