using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ProductDTO
    {

        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public List<StoreDTO> Stores { get; set; } = new List<StoreDTO>();

    }
}
