using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Algorithm;
using Services.Interfaces;
using Services.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaviMark.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TSPController : ControllerBase
    {
        private readonly IAlgorithm _algorithm;
        private readonly IService<ProductDTO> _productServices;
        private readonly IService<StoreDTO> _storeServices;

        public TSPController(IAlgorithm algorithm, IService<ProductDTO> productServices, IService<StoreDTO> storeServices)
        {
            _algorithm = algorithm;
            _productServices = productServices;
            _storeServices = storeServices;
        }


        // POST api/<TSPController>
        [HttpPost]
        public async Task<List<SpReturnDTO>> Post([FromBody] DataDTO data)
        {
            //List<StoreDTO> stores=new List<StoreDTO>();
            List<SpReturnDTO> lst=new List<SpReturnDTO>();
            var stores=await _algorithm.MainAlg(data);
            
            foreach (StoreDTO store in stores) 
            {
                //פונקציה שממירה את הכתובת לX Y
                var location=await CalculateDistancesByGoogleMaps.GetCoordinatesAsync(store.StreatName);
                SpReturnDTO spDTO = new SpReturnDTO(location.Item1, location.Item2, store.StoreName, store.ProductsUse);
                lst.Add(spDTO);
            }
            return lst;
        }
        [HttpGet]
        public async Task<ProductsAndStoresDTO> Get()
        {
            ProductsAndStoresDTO ps = new ProductsAndStoresDTO();
            List<ProductDTO> p=new List<ProductDTO>();
            List<StoreDTO> s=new List<StoreDTO>();
            p = await _productServices.GetAllAsync();
            s = await _storeServices.GetAllAsync();
            foreach(StoreDTO store in s)
            {
                ps.Stores.Add(store.StoreName);
            }
            foreach (ProductDTO product in p)
            {
                ps.Products.Add(product.ProductName);
            }
            return ps;
        }


    }
}
