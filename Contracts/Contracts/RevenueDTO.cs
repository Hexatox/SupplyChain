using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts
{
    public class RevenueDto
    {
        public decimal CurrentTotalRevenue { get; set; }
        public int RevenuePercentage { get; set; }
        public int CurrentSales { get; set; }
        public int SalesPercentage { get; set; }
        public int CurrentTodaySales { get; set; }
        public int TodaySalesPercentage { get; set; }
    }

}
