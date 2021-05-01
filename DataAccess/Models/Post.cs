using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset PublishedAt { get; set; }
        public string ImageUrl { get; set; }
        public int UserByTypeId { get; set; }

        public virtual UserByType UserByType { get; set; }
    }
}
