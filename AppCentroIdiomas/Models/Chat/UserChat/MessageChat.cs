using System;

namespace AppCentroIdiomas.Models
{
    public class MessageChat {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public DateTimeOffset SentAt { get; set; }
        public DateTimeOffset? ReadAt { get; set; }
        public bool IsRead { get; set; }
    }
}
