using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/product-recommendations")]
    [ApiController]
    public class ProductRecommendationController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public ProductRecommendationController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("/{userId}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductsBydescription(int userId, string description = "")
        {
            var query = _dbContext.Products
                .Include(x => x.Images)
                .Include(x => x.ProductType)
                .Include(x => x.ProductPresentation)
                .Include(x => x.User)
                .ThenInclude(x => x.UserInformation)
                .Where(x => x.Description.Contains(description))
                .Take(2);

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
    }
}