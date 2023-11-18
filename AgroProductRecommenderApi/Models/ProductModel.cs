using System;

namespace AgroProductRecommenderApi.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTimeOffset HarvestDate { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductPresentationId { get; set; }
    }
}
