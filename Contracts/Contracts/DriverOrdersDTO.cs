using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts
{
    public class DriverOrdersDTO
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public byte OrderStatus { get; set; }
    }

}
