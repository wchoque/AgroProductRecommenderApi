using AgroProductRecommenderApi.Controllers;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductRecommender.Tests
{
    public class FavoriteControllerTests
    {
        private readonly FavoriteProductController _controller;
        private readonly AgroProductRecommenderDBContext _context;

        public FavoriteControllerTests()
        {
            var dbName = $"TestDatabase_{Guid.NewGuid()}";

            var options = new DbContextOptionsBuilder<AgroProductRecommenderDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            _context = new AgroProductRecommenderDBContext(options);
            _controller = new FavoriteProductController(_context);
        }

        [Fact]
        public void AddToFavorites_ShouldAddProduct()
        {
            var userId = 1; // Ejemplo de usuario
            var productId = 2; // Ejemplo de producto
            _controller.AddToFavorites(userId, productId);

            Assert.True(_context.FavoriteProducts.Any(f => f.UserId == userId && f.ProductId == productId));
        }

        [Fact]
        public void ListFavorites_ShouldReturnFavorites()
        {
            var userId = 1; // Ejemplo de usuario
            var result = _controller.ListFavorites(userId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void RemoveFromFavorites_ShouldRemoveProduct()
        {
            var userId = 1; // Ejemplo de usuario
            var productId = 2; // Ejemplo de producto
            _controller.RemoveFromFavorites(userId, productId);

            Assert.False(_context.FavoriteProducts.Any(f => f.UserId == userId && f.ProductId == productId));
        }
    }
}