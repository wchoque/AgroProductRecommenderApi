using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgroProductRecommenderApi.Models;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/product-types")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public ProductTypeController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductTypeModel>>> GetProductTypes()
        {
            return await _dbContext.ProductTypes
                .Select(x => new ProductTypeModel { Id = x.Id, Name = x.Name })
                .ToListAsync();
        }
    }
}
