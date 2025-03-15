using Backend.Contracts;
using Business_Layer;
using Contracts.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductAPI : ControllerBase
    {
        [HttpGet("GetAllProducts", Name = "GetAllProducts")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProduct()
        {
            List<ProductResponseDTO> result = await clsProduct.GetAllProduct();
            if (result == null) return NotFound("No products found!");
            return Ok(result);
        }


        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        [HttpPost(Name = "AddProduct")]
        public ActionResult<ProductRequestDTO> AddProduct(ProductRequestDTO productRequestDTO)
        {
            if (productRequestDTO.Quantity < 0 || productRequestDTO.Price < 0 || productRequestDTO.Weight < 0 || productRequestDTO.Cost < 0)
                return BadRequest("Invalid product data");
            clsProduct product = new clsProduct(productRequestDTO);
            if (!product.Save()) return StatusCode(500,"Couldn't save the product");
            productRequestDTO.ProductID = productRequestDTO.ProductID;
            return CreatedAtRoute("GetProductByID", new { id = productRequestDTO.ProductID }, productRequestDTO);

        }


        [HttpGet("{id}", Name = "GetProductByID")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]

        public ActionResult<ProductRequestDTO> GetProductByID(int id)
        {
            if (id < 1) return BadRequest();
            clsProduct product = clsProduct.Find(id);
            if (product == null) return NotFound("Product Was Not Found !");
            return Ok(product.productRequestDTO);
        }

    }
}
