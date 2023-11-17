using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRecommendationController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public ProductRecommendationController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/ProductRecommendation?userId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetRecommendedProducts(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            return await _dbContext.Products.Take(10).ToListAsync();
        }
    }
}