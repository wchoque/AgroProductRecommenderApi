namespace AgroProductRecommenderApi.Models
{
    public class UserInformationModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Gender { get; set; }
        public string Bio { get; set; }
        public string WebpageUrl { get; set; }
        public string Dni { get; set; }
        public string ImageUrl { get; set; }
    }
}
