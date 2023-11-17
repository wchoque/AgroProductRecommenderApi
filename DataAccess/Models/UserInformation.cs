using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class UserInformation
    {
        public UserInformation()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}