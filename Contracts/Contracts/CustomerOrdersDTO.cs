using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts
{
    public class CustomerOrdersDTO
    {
        public int OrderID { get; set; }
        public string ProductName {  get; set; }
        public int Weight {  get; set; }
        public int Price { get; set; }
        public int TotalAmount {  get; set; }
        public int Quantity {  get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public byte OrderStatus { get;set; }
        public string? Image { get; set; }
    }
}
