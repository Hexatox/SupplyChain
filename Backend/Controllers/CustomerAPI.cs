using Business_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerAPI : ControllerBase
    {
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [HttpGet("AllCustomers")]
        public ActionResult<IEnumerable<CustomerResponseDTO>> GetCustomers()
        {
            List<CustomerResponseDTO> Customers = clsCustomer.GetAllCustomer();
            if (Customers.Count > 0)
            {
                return Ok(Customers);
            }
            return NotFound("No Customer Data");
        }

    }
}
