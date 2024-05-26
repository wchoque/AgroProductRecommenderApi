using DataAccess.Models;

namespace AgroProductRecommenderApi.Controllers.Orders
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }
        public int ProductChatMessageId{ get; set; }
        public string BuyerName { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductDescription { get; set; }
        public int Quantity { get; set; }
        public string OrderDate { get; set; }
        public string HarvestDate { get; set; }
        public double TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
