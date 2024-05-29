using DataAccess.Models;

namespace AgroProductRecommenderApi.Controllers.Orders
{
    public class OrderRatingDTO
    {
        public int RaterUserId { get; set; }
        public int RatedUserId { get; set; }
        public int Rating { get; set; } // Calificación sobre 5
        public string Comment { get; set; }
    }
}
