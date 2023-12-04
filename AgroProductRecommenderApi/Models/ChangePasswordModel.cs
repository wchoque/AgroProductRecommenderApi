namespace AgroProductRecommenderApi.Models
{
    public class ChangePasswordModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
