#nullable disable

using System;

namespace DataAccess.Models
{
    public partial class OrderRating
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int RaterUserId { get; set; } // Usuario que da la calificación
        public int RatedUserId { get; set; } // Usuario que recibe la calificación
        public int Rating { get; set; } // Calificación sobre 5
        public string Comment { get; set; }
        public DateTime RatedAt { get; set; }

        public virtual Order Order { get; set; }
        public virtual User RaterUser { get; set; }
        public virtual User RatedUser { get; set; }
    }
}