using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaviMark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IService<ProductDTO> _productServices;
        public ProductController(IService<ProductDTO> services)
        {
            _productServices = services;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products= await _productServices.GetAllAsync();
            return Ok(products);
        }

        // GET api/<ProductController>/5
        [HttpGet("{name}")]
        public Task<ProductDTO> Get(string name)
        {
            return _productServices.GetByNameAsync(name);
        }

        // POST api/<ProductController>
        [HttpPost]
        public Task<List<ProductDTO>> Post([FromBody] ProductDTO entity)
        {
            return _productServices.AddAsync(entity);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ProductDTO p)
        {
            _productServices.UpdateAsync(p);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            _productServices.DeleteAsync(name);
        }
    }
}
