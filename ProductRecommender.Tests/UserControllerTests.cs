using AgroProductRecommenderApi.Controllers;
using AgroProductRecommenderApi.Models;
using AgroProductRecommenderApi.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductRecommender.Tests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly AgroProductRecommenderDBContext _context;
        public UserControllerTests()
        {
            var dbName = $"TestDatabase_{Guid.NewGuid()}";

            var options = new DbContextOptionsBuilder<AgroProductRecommenderDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            _context = new AgroProductRecommenderDBContext(options);
            _controller = new UserController(_context);

            _context.Users.Add(new User
            {
                Id = 1,
                UserName = "Test",
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
        public void UpdateProfile_ShouldUpdateInformation()
        {
            var userId = 1; // Ejemplo de usuario
            var updatedInfo = new UserInformationModel
            {
                FirstName = "test",
                LastName = "test",
                Bio = "This is the bio"
            };
            var result = _controller.UpdateProfile(userId, updatedInfo);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
