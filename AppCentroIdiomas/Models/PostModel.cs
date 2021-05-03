using System;

namespace AppCentroIdiomas.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PublishedAt { get; set; }
        public string ImageUrl { get; set; }
        public string PostedBy { get; set; }
    }
}
