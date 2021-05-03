using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class ChatMessage
    {
        public int Id { get; set; }
        public int UserIdFrom { get; set; }
        public int UserIdTo { get; set; }
        public string MessageContent { get; set; }
        public DateTimeOffset SentAt { get; set; }
        public DateTimeOffset? ReadAt { get; set; }

        public virtual User UserIdFromNavigation { get; set; }
        public virtual User UserIdToNavigation { get; set; }
    }
}
