using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts
{
    public class SupplierOrdersDTO
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public byte OrderStatus { get; set; }
        public string DriverName { get; set; }
    }

}
