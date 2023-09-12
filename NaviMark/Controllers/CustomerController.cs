
using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaviMark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IService<CustomerDTO> _customerServices;
        public CustomerController(IService<CustomerDTO> services)
        {
            _customerServices = services;
        }

        // GET: api/<CustomerController>
        [HttpGet]
        public Task<List<CustomerDTO>> Get()
        {
            return _customerServices.GetAllAsync();
        }

        // GET api/<CustomerController>/5
        [HttpGet("{name}")]
        public Task<CustomerDTO> Get(string name)
        {
            return _customerServices.GetByNameAsync(name);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<bool> Post([FromBody] CustomerDTO entity)
        {
            var x=true;
            try 
            {
                var check = await _customerServices.GetByNameAsync(entity.CustomerName);
                x = false;
            }
            catch(Exception ex) 
            {
                x = true;
            }
            if (x)
            {
                var result = await _customerServices.AddAsync(entity);
                if (result != null)
                    return true;
            }
            return false;
        }
        
        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CustomerDTO c)
        {
            _customerServices.UpdateAsync(c);
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            _customerServices.DeleteAsync(name);
        }
    }
}
