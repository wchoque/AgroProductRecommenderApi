namespace AppCentroIdiomas.Models.Chat
{
    public class HistoryChatDTO
    {
        public int UserIdTo { get; set; }
        public string DisplayNameTo { get; set; }
        public string MessageContent { get; set; }
        public string SentAt { get; set; }
        public string ReadAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }
    }
}
