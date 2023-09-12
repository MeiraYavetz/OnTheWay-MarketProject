using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class MaskDTO
    {
        public long Mask { get; set; }
        public double Cost { get; set; }
        public MaskDTO(long mask, double cost)
        {
            Mask = mask;
            Cost = cost;    
        }
    }
}
