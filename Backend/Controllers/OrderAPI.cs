using Business_Layer;
using Contracts.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderAPI : ControllerBase
    {
        [HttpGet("GetTotalRevenuesAsync", Name = "GetTotalRevenuesAsync")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<RevenueDto>> GetTotalRevenuesAsync()
        {
            RevenueDto result = await clsOrder.GetTotalRevenuesAsync();
            if (result == null) return NotFound("Total Revenue In Month Not Found!");
            return Ok(result);
        }


        [HttpGet("GetRecentSalesAsync", Name = "GetRecentSalesAsync")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<RecentSalesDTO>>> GetRecentSalesAsync()
        {
            List<RecentSalesDTO> result = await clsOrder.GetRecentSalesAsync();
            if (result == null) return NotFound("No recent sales found!");
            return Ok(result);
        }


        [HttpGet("GetProductsForAllMonthsAsync", Name = "GetProductsForAllMonthsAsync")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<OrdersPerMonthDTO>>> GetProductsForAllMonthsAsync()
        {
            List<OrdersPerMonthDTO> result = await clsOrder.GetProductsForAllMonthsAsync();
            if (result == null) return NotFound("No orders found!");
            return Ok(result);
        }


    }
}
