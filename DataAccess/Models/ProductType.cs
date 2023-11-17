using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}