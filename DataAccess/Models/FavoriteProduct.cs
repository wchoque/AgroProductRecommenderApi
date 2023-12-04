﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public partial class FavoriteProduct
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //public virtual User User { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
