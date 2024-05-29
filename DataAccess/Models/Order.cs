#nullable disable

using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public enum OrderStatus
    {
        New,
        PaymentCompleted,
        Canceled,
        Completed
    }

    public partial class Order
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int ProductId { get; set; }
        public int ChatMessageId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset HarvestDate { get; set; }
        public double TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public virtual User Buyer { get; set; }
        public virtual User Seller { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductChatMessage ChatMessage { get; set; }
        public virtual ICollection<OrderRating> OrderRatings { get; set; } = new List<OrderRating>();
    }
}