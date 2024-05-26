using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Controllers.Orders;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET: api/orders
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders(int userId)
        {
            var orders = await _dbContext.Orders
                .Include(o => o.Buyer)
                .ThenInclude(user => user.UserInformation)
                .Include(o => o.Product).ThenInclude(product => product.ProductType)
                .Where(x => x.SellerId == userId && (x.Status == OrderStatus.New || x.Status == OrderStatus.PaymentCompleted))
                .ToListAsync();

            var ordersResult = orders.Select(o => new OrderDTO
            {
                OrderId = o.Id,
                ProductChatMessageId = o.ChatMessageId,
                BuyerName = $"{o.Buyer.UserInformation.FirstName} {o.Buyer.UserInformation.LastName}",
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
                BuyerName = $"{o.Buyer.UserInformation.FirstName} {o.Buyer.UserInformation.LastName}",
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
                ProductTypeName = order.Product.ProductType.Name,
                ProductDescription = order.Product.Description,
                Quantity = order.Quantity,
                HarvestDate = order.HarvestDate.ToString("dd/MM/yyyy"),
                OrderDate = order.OrderDate.ToString("dd/MM/yyyy"),
                TotalAmount = order.TotalAmount,
                Status = order.Status
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
    }
}
