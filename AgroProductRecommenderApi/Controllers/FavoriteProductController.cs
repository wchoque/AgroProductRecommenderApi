using System.Linq;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult ListFavorites(int userId)
        {
            var favorites = _dbContext.FavoriteProducts.Where(f => f.UserId == userId).ToList();
            return Ok(favorites);
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