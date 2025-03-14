using Backend.Contracts;
using Business_Layer;
using Contracts.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/ProductAPI")]
    [ApiController]
    public class ProductAPI : ControllerBase
    {
        [HttpGet("GetAllProduct", Name = "GetAllProduct")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProduct()
        {
            List<ProductResponseDTO> result = await clsProduct.GetAllProduct();
            if (result == null) return NotFound("No products found!");
            return Ok(result);
        }


    }
}
