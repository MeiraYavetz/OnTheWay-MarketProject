using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaviMark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly IService<CustomerDTO> _customerServices;
        public SignInController(IService<CustomerDTO> services)
        {
            _customerServices = services;
        }
        // POST api/<SignInController>
        [HttpPost]
        public async Task<bool> Post([FromBody] CustomerDataDTO entity)
        {
            try
            {
                var result = await _customerServices.GetByNameAsync(entity.CustomerName);
                if (result != null && result.Passward == entity.Password) 
                    return true;
                return false;
            }
            catch(Exception ex) 
            {
                return false;
            }
        }

    }
}
