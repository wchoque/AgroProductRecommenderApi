namespace AgroProductRecommenderApi.Models.Chat.ProductChat
{
    public class ProductChatMessageInDto
    {
        public int UserIdFrom { get; set; }
        public int UserIdTo { get; set; }
        public int ProductId { get; set; } = 1;
        public string MessageContent { get; set; }
    }
}
