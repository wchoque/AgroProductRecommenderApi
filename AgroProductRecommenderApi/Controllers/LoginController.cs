using System.Threading.Tasks;
using AgroProductRecommenderApi.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public LoginController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!await IsValidUser(loginModel))
            {
                return Unauthorized();
            }

            var user = await _dbContext.Users
                .Include(x => x.UserInformation)
                .FirstAsync(x =>
                    x.IsActive &&
                    x.UserName.Equals(loginModel.UserName) &&
                    x.Password.Equals(loginModel.Password));

            var userInformation = new LoggedUserInformation
            {
                Id = user.Id,
                FirstName = user.UserInformation.FirstName,
                LastName = user.UserInformation.LastName,
                Avatar = user.AvatarUrl,
                Email = user.UserInformation.Email,
                DisplayName = string.Concat(user.UserInformation.FirstName, " ", user.UserInformation.LastName)
            };

            return Ok(userInformation);
        }

        private async Task<bool> IsValidUser(LoginModel loginModel)
        {
            return await _dbContext.Users.AnyAsync(x =>
                x.IsActive &&
                x.UserName.Equals(loginModel.UserName) &&
                x.Password.Equals(loginModel.Password));
        }
    }
}