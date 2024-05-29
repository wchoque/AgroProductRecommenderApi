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
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public OrderController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //// GET: api/orders/users/{userId}
        //[HttpGet("users/{userId}")]
        //public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders(int userId)
        //{
        //    var orders = await _dbContext.Orders
        //        .Include(o => o.Buyer)
        //        .ThenInclude(user => user.UserInformation)
        //        .Include(o => o.Product).ThenInclude(product => product.ProductType)
        //        .Include(order => order.OrderRatings).Include(order => order.Seller)
        //        .Where(x => x.SellerId == userId && (x.Status == OrderStatus.New || x.Status == OrderStatus.PaymentCompleted))
        //        .ToListAsync();

        //    var ordersResult = orders.Select(o => new OrderDTO
        //    {
        //        OrderId = o.Id,
        //        ProductChatMessageId = o.ChatMessageId,
        //        OtherUserName = o.SellerId == userId ?
        //            $"{o.Buyer.UserInformation.FirstName} {o.Buyer.UserInformation.LastName}" :
        //            $"{o.Seller.UserInformation.FirstName} {o.Seller.UserInformation.LastName}",
        //        OtherUserId = o.SellerId == userId ? o.Buyer.Id : o.Seller.Id,
        //        ProductTypeName = o.Product.ProductType.Name,
        //        ProductDescription = o.Product.Description,
        //        Quantity = o.Quantity,
        //        HarvestDate = o.HarvestDate.ToString("dd/MM/yyyy"),
        //        OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
        //        TotalAmount = o.TotalAmount,
        //        Status = o.Status,
        //        Rating = o.OrderRatings.FirstOrDefault(r => r.RaterUserId == userId)?.Rating ?? 0
        //    }).ToList();

        //    return Ok(ordersResult);
        //}
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
                            (o.Status == OrderStatus.New || o.Status == OrderStatus.PaymentCompleted))
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


        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.Buyer)
                .ThenInclude(user => user.UserInformation)
                .Include(o => o.Product).ThenInclude(product => product.ProductType)
                .Where(x => x.Status == OrderStatus.New || x.Status == OrderStatus.PaymentCompleted)
                .ToListAsync();

            var ordersResult = orders.Select(o => new OrderDTO
            {
                OrderId = o.Id,
                ProductChatMessageId = o.ChatMessageId,
                OtherUserName = $"{o.Buyer.UserInformation.FirstName} {o.Buyer.UserInformation.LastName}",
                ProductTypeName = o.Product.ProductType.Name,
                ProductDescription = o.Product.Description,
                Quantity = o.Quantity,
                HarvestDate = o.HarvestDate.ToString("dd/MM/yyyy"),
                OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
                TotalAmount = o.TotalAmount,
                Status = o.Status
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

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(int chatMessageId)
        {
            var chatMessage = await _dbContext.ProductChatMessages
                .Include(c => c.UserIdFromNavigation)
                .Include(c => c.UserIdToNavigation)
                .Include(c => c.ProductIdToNavigation)
                .FirstOrDefaultAsync(c => c.Id == chatMessageId);

            if (chatMessage == null)
            {
                return BadRequest("Chat message not found");
            }

            var order = new Order
            {
                BuyerId = chatMessage.UserIdTo,
                SellerId = chatMessage.UserIdFrom,
                ProductId = chatMessage.ProductId,
                ChatMessageId = chatMessageId,
                Quantity = chatMessage.ProductIdToNavigation.Quantity,
                TotalAmount = chatMessage.ProductIdToNavigation.Price,
                HarvestDate = chatMessage.ProductIdToNavigation.HarvestDate,
                OrderDate = DateTimeOffset.UtcNow,
                Status = OrderStatus.New
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderUpdateModel model)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Quantity = model.Quantity;
            order.TotalAmount = model.TotalAmount;
            order.HarvestDate = DateTime.Parse(model.HarvestDate);
            order.Status = model.Status;

            _dbContext.Entry(order).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _dbContext.Orders.Any(e => e.Id == id);
        }

        // POST: api/orders/{id}/rate
        [HttpPost("{id}/rate")]
        public async Task<ActionResult> RateOrder(int id, OrderRatingDTO ratingDto)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderRatings)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            var existingRating = order.OrderRatings
                .FirstOrDefault(r => r.RaterUserId == ratingDto.RaterUserId);

            if (existingRating != null)
            {
                existingRating.Rating = ratingDto.Rating;
                existingRating.Comment = ratingDto.Comment;
                existingRating.RatedAt = DateTime.UtcNow;
            }
            else
            {
                //('Admin')--1
                //('Comprador Mayorista')--2
                //('Productor Agricola')--3
                
                //if (ratingDto.UserTypeId == 2)
                //{
                //}

                var rating = new OrderRating
                {
                    OrderId = id,
                    RaterUserId = ratingDto.RaterUserId,
                    RatedUserId = ratingDto.RatedUserId,
                    Rating = ratingDto.Rating,
                    Comment = ratingDto.Comment,
                    RatedAt = DateTime.UtcNow
                };

                _dbContext.OrderRatings.Add(rating);
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
