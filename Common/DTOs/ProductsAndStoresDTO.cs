using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class ProductsAndStoresDTO
    {
        public List<string>? Products { get; set; } = new List<string>();
        public List<string>? Stores { get; set; } = new List<string>();

    }
}
