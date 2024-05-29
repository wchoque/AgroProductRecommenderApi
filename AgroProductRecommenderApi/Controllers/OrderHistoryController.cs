using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using AgroProductRecommenderApi.Controllers.Orders;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/orders-history")]
    [ApiController]
    public class OrderHistoryController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public OrderHistoryController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/orders/users/{userId}
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders(int userId)
        {
            var orders = await _dbContext.Orders
                .Include(o => o.Buyer)
                .ThenInclude(user => user.UserInformation)
                .Include(o => o.Seller)
                .ThenInclude(user => user.UserInformation)
                .Include(o => o.Product)
                .ThenInclude(product => product.ProductType)
                .Include(o => o.OrderRatings)
                .Where(o => (o.SellerId == userId || o.BuyerId == userId) &&
                            o.Status == OrderStatus.Completed)
                .ToListAsync();

            var ordersResult = orders.Select(o => new OrderDTO
            {
                OrderId = o.Id,
                ProductChatMessageId = o.ChatMessageId,
                OtherUserName = o.SellerId == userId ?
                    $"{o.Buyer.UserInformation.FirstName} {o.Buyer.UserInformation.LastName}" :
                    $"{o.Seller.UserInformation.FirstName} {o.Seller.UserInformation.LastName}",
                OtherUserId = o.SellerId == userId ? o.Buyer.Id : o.Seller.Id,
                ProductTypeName = o.Product.ProductType.Name,
                ProductDescription = o.Product.Description,
                Quantity = o.Quantity,
                HarvestDate = o.HarvestDate.ToString("dd/MM/yyyy"),
                OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Rating = o.OrderRatings.FirstOrDefault(r => r.RaterUserId == userId)?.Rating ?? 0
            }).ToList();

            return Ok(ordersResult);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailDTO>> GetOrder(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Buyer).ThenInclude(user => user.UserInformation)
                .Include(o => o.Seller)
                .Include(o => o.Product).ThenInclude(product => product.ProductType)
                .Include(o => o.ChatMessage)
                .Include(o => o.OrderRatings)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var ordersResult = new OrderDetailDTO
            {
                OrderId = order.Id,
                ProductChatMessageId = order.ChatMessageId,
                BuyerName = $"{order.Buyer.UserInformation.FirstName} {order.Buyer.UserInformation.LastName}",
                BuyerUserId = order.Buyer.Id,
                ProductTypeName = order.Product.ProductType.Name,
                ProductDescription = order.Product.Description,
                Quantity = order.Quantity,
                HarvestDate = order.HarvestDate.ToString("dd/MM/yyyy"),
                OrderDate = order.OrderDate.ToString("dd/MM/yyyy"),
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                //Rating = order.OrderRatings.FirstOrDefault(r => r.RaterUserId == userId)?.Rating ?? 0
                //Ratings = order.OrderRatings.Select(r => new OrderRatingDTO
                //{
                //    RaterUserId = r.RaterUserId,
                //    RatedUserId = r.RatedUserId,
                //    Rating = r.Rating,
                //    Comment = r.Comment,
                //    RatedAt = r.RatedAt.ToString("dd/MM/yyyy HH:mm:ss")
                //}
                //).ToList()
            };

            return ordersResult;
        }
    }
}
