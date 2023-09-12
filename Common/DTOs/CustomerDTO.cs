using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Passward { get; set; } = null!;


    }
}
