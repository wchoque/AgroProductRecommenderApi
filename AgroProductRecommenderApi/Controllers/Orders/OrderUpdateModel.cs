using DataAccess.Models;

namespace AgroProductRecommenderApi.Controllers.Orders
{
    public class OrderUpdateModel
    {
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public string HarvestDate { get; set; }
        public OrderStatus Status { get; set; }
    }
}
