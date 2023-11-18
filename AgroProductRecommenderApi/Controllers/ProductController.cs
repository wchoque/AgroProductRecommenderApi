using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/products")]
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
        [HttpGet("filtered-by-user")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductsByUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            
            if (user == null)
            {
                return NotFound("User not found");
            }

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



        //TODO Implement handle images

        [HttpPost("upload-images")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files, [FromForm] int id)
        {
            var product = _dbContext.Products
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    continue;
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    var image = new ProductImage
                    {
                        ImageData = memoryStream.ToArray(),
                        Product = product,
                    };

                    product.Images.Add(image);
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok(new { product.Id });
        }

        [HttpGet("{id}/get-images")]
        public async Task<ActionResult<IEnumerable<string>>> GetImages(int id)
        {
            var product = await _dbContext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            var imageUrls = product.Images
                .Select(img => Url.Action(nameof(GetImage), new { imageId = img.Id })).ToList();

            return Ok(imageUrls);
        }

        [HttpGet("get-image/{imageId}")]
        public IActionResult GetImage(int imageId)
        {
            var image = _dbContext.ProductImages.Find(imageId);

            if (image == null || image.ImageData == null)
            {
                return NotFound("Image not found");
            }

            return File(image.ImageData, "image/jpeg");
        }

        private bool ProductExists(int id)
        {
            return _dbContext.Products.Any(e => e.Id == id);
        }
    }
}