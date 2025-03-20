using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts
{
    public class DeliveringOrders
    {
        public int OrderId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int ServiceTime { get; set; } = 10;// in minutes
        public string Address { get;set; }

        public DeliveringOrders(int OrderId , double Latitude , double Longitude, int ServiceTime,string Address) { 
            this.OrderId = OrderId;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.ServiceTime = ServiceTime;
            this.Address = Address;
        }

        public DeliveringOrders() { }
    }
}
