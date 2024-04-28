using System;

namespace AgroProductRecommenderApi.Models.Chat.UserChat
{
    public class ChatMessageDTO
    {
        public string Message { get; set; }
        public int SenderId { get; set; }
        public DateTime timestamp { get; set; }
    }
}