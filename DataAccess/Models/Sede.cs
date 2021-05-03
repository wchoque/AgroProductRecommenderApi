using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Sede
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }
}
