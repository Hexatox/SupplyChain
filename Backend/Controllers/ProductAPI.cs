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
        //For the customer
        [HttpGet("GetAllProducts", Name = "GetAllProducts")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProduct()
        {
            List<ProductResponseDTO> result = await clsProduct.GetAllProduct();
            if (result == null) return NotFound("No products found!");
            return Ok(result);
        }

        //For the supplier
        [HttpGet("GetAllProductsForSupplier/{SupplierID}", Name = "GetAllProductsForSupplier")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProductsForSupplier(int SupplierID)
        {
            if (SupplierID < 0) return BadRequest("SupplierID should be greater than 0");
            List<ProductResponseDTO> result = await clsProduct.GetAllProductsForSupplier(SupplierID);
            if (result == null) return NotFound("No products found!");
            return Ok(result);
        }


        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        [HttpPost(Name = "AddProduct")]
        public ActionResult<ProductRequestDTO> AddProduct(ProductRequestDTO productRequestDTO )
        {
            if (productRequestDTO.Quantity < 0 || productRequestDTO.Price < 0 || productRequestDTO.Weight < 0 || productRequestDTO.Cost < 0)
                return BadRequest("Invalid product data");
            productRequestDTO.Image = clsUtil.SaveImage(productRequestDTO.file);
            clsProduct product = new clsProduct(productRequestDTO);
            if (!product.Save()) return StatusCode(500,"Couldn't save the product");
            productRequestDTO.ProductID = product.ProductID;
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


        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        [HttpDelete("{id}", Name = "Delete")]
        public ActionResult DeleteProduct(int id)
        {
            if (id < 1) return BadRequest("ID should be greater than 0");
            clsProduct product = clsProduct.Find(id);
            if (product == null) return NotFound("Product Not Found !");
            if (product.Delete())
            {
                clsUtil.DeleteImage(product.Image);
                return Ok("Product Deleted Successfully");
            }
            else
            {
                return BadRequest("Cannot Delete the product because it is" +
                    " linked to other tables");
            }
        }



        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}", Name = "Update")]
        public ActionResult<ProductRequestDTO> UpdateProduct(int id, ProductRequestDTO productRequestDTO)
        {
            if (productRequestDTO == null || string.IsNullOrEmpty(productRequestDTO.ProdcutName) || productRequestDTO.Quantity < 0
                || productRequestDTO.Cost < 0 || productRequestDTO.Price < 0 || productRequestDTO.Weight < 0)
                return BadRequest("Product data is not valid !");
            clsProduct product = clsProduct.Find(id);
            if (product == null) return NotFound($"Product With ID = {id} Was Not Found ! ");
            product.ProdcutName = productRequestDTO.ProdcutName;
            product.Weight = productRequestDTO.Weight;
            product.Quantity = productRequestDTO.Quantity;
            product.Price = productRequestDTO.Price;
            product.Cost = productRequestDTO.Cost;
            product.Description = productRequestDTO.Description;
            if(product.Image != productRequestDTO.Image)
            {
                clsUtil.DeleteImage(product.Image);
                product.Image = clsUtil.SaveImage(productRequestDTO.file);

            }
            //product.SupplierID = productRequestDTO.SupplierID;

            product.Save();
            return Ok(product.productRequestDTO);

        }

    }
}
