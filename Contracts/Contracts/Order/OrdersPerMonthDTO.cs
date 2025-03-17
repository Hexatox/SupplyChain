using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts.Order
{
    public class OrdersPerMonthDTO
    {
        public int Month { get; set; }
        public int Orders { get; set; }
    }
}
