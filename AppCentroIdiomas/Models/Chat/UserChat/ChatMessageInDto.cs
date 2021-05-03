namespace AppCentroIdiomas.Models.Chat.UserChat
{
    public class ChatMessageInDto
    {
        public int UserIdFrom { get; set; }
        public int UserIdTo { get; set; }
        public string MessageContent { get; set; }
    }
}
