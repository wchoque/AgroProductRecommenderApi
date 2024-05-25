using DataAccess.Models;

namespace AgroProductRecommenderApi.Models
{
    public class LoggedUserInformation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserType { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string DisplayName { get; set; }
        public UserAccountStatus UserAccountStatus { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}