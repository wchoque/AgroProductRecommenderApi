namespace ProductRecommender
{
    public class ProductRating
    {
        public float UserId { get; set; }
        public float ProductId { get; set; }
        public float Label { get; set; }
    }

    public class ProductPrediction
    {
        public float Score { get; set; }
    }
}