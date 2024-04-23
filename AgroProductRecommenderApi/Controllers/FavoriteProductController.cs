using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [ApiController]
    [Route("api/favorite-products")]
    public class FavoriteProductController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public FavoriteProductController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("list/{userId}")]
        public async Task<IActionResult> ListFavorites(int userId) 
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var productsIds = await _dbContext.FavoriteProducts.Where(f => f.UserId == userId).Select(x => x.ProductId).ToListAsync();

            var query = _dbContext.Products
                .Include(x => x.Images)
                .Include(x => x.ProductType)
                .Include(x => x.ProductPresentation)
                .Include(x => x.User)
                .ThenInclude(x => x.UserInformation)
                .Where(x => x.UserId == user.Id && productsIds.Contains(x.Id));

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

        [HttpPost("add")]
        public IActionResult AddToFavorites(int userId, int productId)
        {
            var favorite = new FavoriteProduct { UserId = userId, ProductId = productId };
            _dbContext.FavoriteProducts.Add(favorite);
            _dbContext.SaveChanges();

            return Ok();
        }
        
        [HttpPost("remove")]
        public IActionResult RemoveFromFavorites(int userId, int productId)
        {
            var favorite = _dbContext.FavoriteProducts.FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
            if (favorite == null) return NotFound();

            _dbContext.FavoriteProducts.Remove(favorite);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}