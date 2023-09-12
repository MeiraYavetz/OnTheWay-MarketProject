using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class StoreDTO
    {

        public int StoreId { get; set; }

        public string StoreName { get; set; } = null!;

        public string StreatName { get; set; } = null!;

        public double priority { get; set; } = 0;

        public List<ProductDTO>? Products { get; set; } = new List<ProductDTO>();
        public List<String>? ProductsUse { get; set; } = new List<String>();


    }
}
