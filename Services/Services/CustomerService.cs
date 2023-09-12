using AutoMapper;
using Repositories.Entities;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTOs;
using Repositories.Interfaces;
using AutoMapper.Execution;

namespace Services.Services
{
    public class CustomerService : IService<CustomerDTO>
    {
        private readonly IRepository<Customer> _repository;

        private readonly IMapper _mapper;
        public CustomerService(IMapper mapper, IRepository<Customer> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<CustomerDTO>> AddAsync(CustomerDTO entity)
        {
            var newOne = await _repository.AddAsync(_mapper.Map<Customer>(entity));
            return _mapper.Map<List<CustomerDTO>>(newOne);
            
        }

        public async Task DeleteAsync(string name)
        {
            var customer = await _repository.GetByNameAsync(name);
            await _repository.DeleteAsync(customer.CustomerId); 
        }


        public async Task<List<CustomerDTO>> GetAllAsync()
        {
            return _mapper.Map<List<CustomerDTO>>(await _repository.GetAllAsync());


        }

        public async Task<CustomerDTO> GetByNameAsync(string name)
        {
            return _mapper.Map<CustomerDTO>(await _repository.GetByNameAsync(name));

        }
        public async Task<CustomerDTO> GetByIdAsync(int id)
        {
            return _mapper.Map<CustomerDTO>(await _repository.GetByIdAsync(id));

        }
        public async Task UpdateAsync(CustomerDTO entity)
        {
            await _repository.UpdateAsync(_mapper.Map<Customer>(entity));
        }
    }
}
