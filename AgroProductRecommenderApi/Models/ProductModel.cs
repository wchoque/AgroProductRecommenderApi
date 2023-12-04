using System;

namespace AgroProductRecommenderApi.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string HarvestDate { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string DefaultImageUrl { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductPresentationUnit { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductPresentationId { get; set; }
    }
}
