using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class ProductPresentation
    {
        public int Id { get; set; }
        public string Unit { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}