using System;

namespace AppCentroIdiomas.Models
{
    public class AvailableUser {
        public int UserIdTo { get; set; }
        public string DisplayNameTo { get; set; }
        public string LastMessageContent { get; set; }
        public DateTimeOffset? LastMessageSentAt { get; set; }
        public string RoleTo { get; set; }
    }
}
