using System;

#nullable disable

namespace DataAccess.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTimeOffset HarvestDate { get; set; }
        public string ImageUrl { get; set; }
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
        public int ProductPresentationId { get; set; }
        public ProductPresentation ProductPresentation { get; set; }
    }
}