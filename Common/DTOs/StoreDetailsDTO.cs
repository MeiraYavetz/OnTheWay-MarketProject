using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class StoreDetailsDTO
    {
        public int StoreId { get; set; }

        public string StoreName { get; set; } = null!;

        public string StreatName { get; set; } = null!;

        public List<String>? Products { get; set; } = new List<String>();

    }
}
