using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class SpReturnDTO
    {
        public String NameOfStore { get; set; } 
        public double GeoX { get; set; }
        public double GeoY { get; set; }
        public List<String> Products { get; set; } 
        public SpReturnDTO(double geoX, double geoY,String nameOfStore, List<String> products)
        {
            GeoX = geoX;
            GeoY = geoY;
            NameOfStore = nameOfStore;
            Products = products;
            
        }
        public SpReturnDTO(String nameOfStore, List<String> products)
        {
            
            NameOfStore = nameOfStore;
            Products = products;

        }
    }
}
