using System;

#nullable disable

namespace DataAccess.Models
{
    public partial class ProductImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}