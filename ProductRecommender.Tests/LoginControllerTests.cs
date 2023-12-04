using AgroProductRecommenderApi.Controllers;
using AgroProductRecommenderApi.Models;
using AgroProductRecommenderApi.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductRecommender.Tests
{
    public class LoginControllerTests
    {
        private readonly LoginController _controller;
        private readonly AgroProductRecommenderDBContext _context;

        public LoginControllerTests()
        {
            var dbName = $"TestDatabase_{Guid.NewGuid()}";

            var options = new DbContextOptionsBuilder<AgroProductRecommenderDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            _context = new AgroProductRecommenderDBContext(options);
            _controller = new LoginController(_context);


            _context.Users.Add(new User
            {
                Id = 1,
                UserName = "testuser",
                Password = PasswordHasher.HashPassword("testpassword"),
                IsActive = true,
                UserInformation = new UserInformation
                {
                    FirstName = "base",
                    LastName = "base",
                    Bio = "This is the base bio"
                }
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenUserIsValid()
        {
            var loginModel = new LoginModel { UserName = "testuser", Password = "testpassword" };
            var result = await _controller.Login(loginModel);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnOk_WhenPasswordIsChanged()
        {
            var changePasswordModel = new ChangePasswordModel { UserName = "testuser", CurrentPassword = "testpassword", NewPassword = "newpassword" };
            //var hashedOldPassword = PasswordHasher.HashPassword("testpassword");
            //var user = new User { UserName = "testuser", Password = hashedOldPassword, IsActive = true };

            var result = await _controller.ChangePassword(changePasswordModel);

            Assert.IsType<OkResult>(result);
        }
    }
}
