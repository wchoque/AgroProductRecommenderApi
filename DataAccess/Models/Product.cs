using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Product
    {
        public Product()
        {
            Images = new HashSet<ProductImage>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTimeOffset HarvestDate { get; set; }
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
        public int ProductPresentationId { get; set; }
        public ProductPresentation ProductPresentation { get; set; }
        public ICollection<ProductImage> Images { get; set; }
    }
}