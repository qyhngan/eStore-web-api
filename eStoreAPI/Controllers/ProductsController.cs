using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DataAccess.DTO.Product;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        IProductRepository _productRepository = new ProductRepository();

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts(string searchString = null, string searchPrice = null)
        {
            List<ProductResponse> productResponse = null;
            try
            {
                productResponse = _productRepository.GetAllProduct(searchString, searchPrice).ToList();

                if (productResponse == null)
                {
                    return NotFound("List is empty");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Ok(productResponse);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(int id)
        {
            
            var product = _productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound("Product doesn't exist");
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductResponse product)
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "admin")
            {
                return Unauthorized();
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            try
            {
                _productRepository.UpdateProduct(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductRequest product)
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "admin")
            {
                return Unauthorized();
            }

            try
            {
                _productRepository.AddProduct(product);
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteProduct(int id)
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "admin")
            {
                return Unauthorized();
            }

            var product = _productRepository.FindProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.DeleteProduct(id);

            return Ok("Delete successfully");
        }

        private bool ProductExists(int id)
        {
            return _productRepository.FindProductById(id) != null;
        }
    }
}
