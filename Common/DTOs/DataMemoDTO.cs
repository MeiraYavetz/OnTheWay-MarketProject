using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class DataMemoDTO
    {
        public double Cost { get; set; }
        public int LastStoreId { get; set; }
        public int PrevStoreId { get; set; }
        public DataMemoDTO(double cost, int lastStoreId, int prevStoreId)
        {
            Cost = cost;
            LastStoreId = lastStoreId;
            PrevStoreId = prevStoreId;
        }
    }
}
