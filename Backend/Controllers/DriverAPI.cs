using Business_Layer;
using Contracts.Contracts;
using Contracts.Contracts.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverAPI : ControllerBase
    {

        [HttpGet("GetDriverOrders/{DriverID}", Name = "GetDriverOrders")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<List<DriverOrdersDTO>>> GetDriverOrders(int DriverID)
        {
            if (DriverID < 0)
            {
                return BadRequest("DriverID should be greater than 0");
            }
            List<DriverOrdersDTO> result = await clsDriver.GetDriverOrders(DriverID);
            if (result == null) return NotFound("No orders found!");
            return Ok(result);
        }

    }
}
