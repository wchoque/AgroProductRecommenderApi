using System;

namespace AppCentroIdiomas.Models
{
    public class AvailableUser {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string LastMessage { get; set; }
        public DateTimeOffset? LastMessageAt { get; set; }
    }
}
