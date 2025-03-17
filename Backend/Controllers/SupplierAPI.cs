using Business_Layer;
using Contracts.Contracts;
using Contracts.Contracts.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierAPI : ControllerBase
    {

        [HttpGet("GetSupplierOrders/{SupplierID}", Name = "GetSupplierOrders")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<List<SupplierOrdersDTO>>> GetSupplierOrders(int SupplierID)
        {
            if (SupplierID < 0)
            {
                return BadRequest("SupplierID should be greater than 0");
            }
            List<SupplierOrdersDTO> result = await clsSupplier.GetSupplierOrders(SupplierID);
            if (result == null) return NotFound("No orders found!");
            return Ok(result);
        }

    }
}
