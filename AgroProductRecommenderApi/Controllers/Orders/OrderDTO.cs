using DataAccess.Models;

namespace AgroProductRecommenderApi.Controllers.Orders
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int ProductChatMessageId { get; set; }
        public string OtherUserName { get; set; } // Nuevo campo para el nombre del otro usuario
        public int OtherUserId { get; set; } // Nuevo campo para el ID del otro usuario
        public string ProductTypeName { get; set; }
        public string ProductDescription { get; set; }
        public int Quantity { get; set; }
        public string HarvestDate { get; set; }
        public string OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int Rating { get; set; }
    }
}
