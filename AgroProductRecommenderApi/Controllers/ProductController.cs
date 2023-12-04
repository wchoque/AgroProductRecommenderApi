using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductsByUser(int userId, string description = "")
        {
            var user = await _dbContext.Users.FindAsync(userId);
            
            if (user == null)
            {
                return NotFound("User not found");
            }

            var query = _dbContext.Products
                .Include(x => x.Images)
                .Include(x => x.ProductType)
                .Include(x => x.ProductPresentation)
                .Include(x => x.User)
                .ThenInclude(x => x.UserInformation)
                .Where(x => x.UserId == user.Id);

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(x => x.Description.Contains(description));
            }

            var products = await query
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Description = x.Description,
                    Location = x.Location,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    HarvestDate = x.HarvestDate.ToString("G"),
                    CreatedAt = x.CreatedAt.ToString("G"),
                    CreatedBy = $"{x.User.UserInformation.FirstName} {x.User.UserInformation.LastName}",
                    ProductTypeName = x.ProductType.Name,
                    ProductPresentationUnit = x.ProductPresentation.Unit,
                    //DefaultImageUrl = $"https://lm-test-d-aze2-app-001.azurewebsites.net/api/products/get-image/{x.Images.FirstOrDefault().Id}",
                    DefaultImageUrl = "https://i0.wp.com/diarioelpueblo.com.pe/wp-content/uploads/2022/03/2-Exportan-cebolla-a-Ecuador_.jpg?w=748&ssl=1",
                    //DefaultImageUrl = Url.Action(nameof(GetImage), new { imageId = x.Images.FirstOrDefault().Id }),
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
        public async Task<IActionResult> PutCourse(int id, ProductModel product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var existingProduct = await _dbContext.Products.FindAsync(id);

            existingProduct.Description = product.Description;
            existingProduct.Location = product.Location;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Price = product.Price;
            existingProduct.HarvestDate = DateTime.Parse(product.HarvestDate);
            existingProduct.ProductTypeId = product.ProductTypeId;
            existingProduct.ProductPresentationId = product.ProductPresentationId;
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
                Description = productModel.Description,
                Location = productModel.Location,
                Quantity = productModel.Quantity,
                Price = productModel.Price,
                HarvestDate = DateTime.Parse(productModel.HarvestDate),
                CreatedAt = DateTimeOffset.Now,
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

            return Ok(new { imageUrls});
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