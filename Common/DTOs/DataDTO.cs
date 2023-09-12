using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class DataDTO
    {
        public String startingPlace { get; set; } = " ";
        public List<String> stores { get; set; } = new List<String>();
        public List<String> products { get; set; } = new List<String>();
    }
}
