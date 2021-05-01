using AppCentroIdiomas.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppCentroIdiomas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppCentroEstudiosDBContext _dbContext;

        public LoginController(AppCentroEstudiosDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (!IsValid(loginModel)) {
                return Unauthorized();
            }

            var userInformation = _dbContext.Users
                .Where(x => x.IsActive && x.UserName.Equals(loginModel.UserName) && x.Password.Equals(loginModel.Password))
                .Select(x => new LoggedUserInformation
                {
                    Id = x.Id,
                    FirstName = x.UserInformation.FirstName,
                    LastName = x.UserInformation.LastName,
                    Avatar = x.AvatarUrl,
                    Email = x.UserInformation.Email,
                    DisplayName = string.Concat(x.UserInformation.FirstName, " ", x.UserInformation.LastName)
                })
                .FirstOrDefault();
            return Ok(userInformation);
        }

        private bool IsValid(LoginModel loginModel) {
            return _dbContext.Users.Any(x => x.IsActive && x.UserName.Equals(loginModel.UserName) && x.Password.Equals(loginModel.Password));
        }
    }
}
