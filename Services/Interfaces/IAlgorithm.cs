using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAlgorithm
    {
        public Task<List<StoreDTO>> MainAlg(DataDTO Lists);
    }
}
