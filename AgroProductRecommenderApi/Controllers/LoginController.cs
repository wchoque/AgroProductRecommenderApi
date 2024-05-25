using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Models;
using AgroProductRecommenderApi.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/login")]
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
            var foundUser = await _dbContext.Users.FirstAsync(x =>
                x.IsActive &&
                x.UserName.Equals(loginModel.UserName));

            if (foundUser == null)
            {
                return NotFound();
            }

            if (!PasswordHasher.VerifyPassword(foundUser.Password, loginModel.Password))
            {
                return Unauthorized();
            }

            var user = await _dbContext.Users
                .Include(x => x.UserInformation)
                .Include(x => x.UserByTypes)
                .FirstAsync(x =>
                    x.IsActive &&
                    x.UserName.Equals(loginModel.UserName));

            var userType = user.UserByTypes.FirstOrDefault(x => x.UserId == user.Id)!.UserTypeId;
            var userInformation = new LoggedUserInformation
            {
                Id = user.Id,
                UserId = user.Id,
                UserName = user.UserName,
                UserType = userType,
                FirstName = user.UserInformation.FirstName,
                LastName = user.UserInformation.LastName,
                Avatar = user.AvatarUrl,
                Email = user.UserInformation.Email,
                DisplayName = string.Concat(user.UserInformation.FirstName, " ", user.UserInformation.LastName),
                UserAccountStatus = user.AccountStatus
            };

            var imageUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("GetProfilePicture", "User", new { id = user.Id })}";
            userInformation.ProfileImageUrl = imageUrl;

            return Ok(userInformation);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            var user = await _dbContext.Users
                .Include(x => x.UserInformation)
                .FirstOrDefaultAsync(x => x.IsActive && x.UserName.Equals(changePasswordModel.UserName));

            if (user == null)
            {
                return NotFound();
            }

            if (!PasswordHasher.VerifyPassword(user.Password, changePasswordModel.CurrentPassword))
            {
                return Unauthorized();
            }

            user.Password = HashPassword(changePasswordModel.NewPassword);
            await _dbContext.SaveChangesAsync();

            var userInformation = new LoggedUserInformation
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.UserInformation.FirstName,
                LastName = user.UserInformation.LastName,
                Avatar = user.AvatarUrl,
                Email = user.UserInformation.Email,
                DisplayName = string.Concat(user.UserInformation.FirstName, " ", user.UserInformation.LastName)
            };
            return Ok(userInformation);
        }

        [HttpGet("get-password/{password}")]
        public async Task<IActionResult> GetPassword(string password)
        {
            return Ok(new { Password = HashPassword(password) });
        }


        private string HashPassword(string password)
        {
            return PasswordHasher.HashPassword(password);
        }
    }
}