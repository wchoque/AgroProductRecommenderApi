using Microsoft.AspNetCore.Http;

namespace AgroProductRecommenderApi.Models
{
    public class CreateUserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public int UserTypeId { get; set; }
        public CreateUserInformationModel UserInformation { get; set; }
    }

    public class CreateUserInformationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
