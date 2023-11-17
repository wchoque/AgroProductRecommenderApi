using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgroProductRecommenderApi.Models;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPresentationController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public ProductPresentationController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductPresentationModel>>> GetProductTypes()
        {
            return await _dbContext.ProductPresentations
                .Select(x => new ProductPresentationModel { Id = x.Id, Unit = x.Unit })
                .ToListAsync();
        }
    }
}
