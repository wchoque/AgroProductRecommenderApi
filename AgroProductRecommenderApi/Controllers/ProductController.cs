using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public ProductController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //// GET: api/Products
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        //{
        //    return await _dbContext.Products.ToListAsync();
        //}


        // GET: api/Product?userId=5
        [HttpGet("GetProductsByUser")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductsByUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            
            var products = await _dbContext.Products
                .Where(x => x.UserId == user.Id)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    HarvestDate = x.HarvestDate,
                    ImageUrl = x.ImageUrl,
                    ProductTypeId = x.ProductTypeId,
                    ProductPresentationId = x.ProductPresentationId
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/Products
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Product>> PostCourse(ProductModel productModel)
        {
            var product = new Product
            {
                UserId = productModel.UserId,
                Name = productModel.Name,
                Quantity = productModel.Quantity,
                Price = productModel.Price,
                HarvestDate = productModel.HarvestDate,
                ImageUrl = productModel.ImageUrl,
                ProductTypeId = productModel.ProductTypeId,
                ProductPresentationId = productModel.ProductPresentationId
            };
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _dbContext.Products.Any(e => e.Id == id);
        }
    }
}