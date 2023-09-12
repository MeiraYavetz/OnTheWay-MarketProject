using Common.DTOs;
using GoogleMapsApi.Entities.Directions.Response;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaviMark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IService<StoreDTO> _storeServices;
        private readonly IServiceStore<StoreDTO> _store;
        
        public StoreController(IService<StoreDTO> services,IServiceStore<StoreDTO> service)
        {
            _storeServices = services;
            _store = service;
        }
        // GET: api/<StoreController>
        [HttpGet]
        public Task<List<StoreDTO>> Get()
        {
            return _storeServices.GetAllAsync();
        }

        // GET api/<StoreController>/5
        [HttpGet("{name}")]
        public Task<StoreDTO> Get(string name)
        {
            return _storeServices.GetByNameAsync(name);
        }

        // POST api/<StoreController>
        [HttpPost]
        public Task<List<StoreDTO>> Post([FromBody] StoreDetailsDTO entity)
        {
            return _store.AddAsync(entity);
        }

        // PUT api/<StoreController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] StoreDTO s)
        {
            _storeServices.UpdateAsync(s);
        }

        // DELETE api/<StoreController>/5
        [HttpDelete("{name}")]
        public async Task Delete(string name)
        {
            
            await _storeServices.DeleteAsync(name);
        }
    }
}
