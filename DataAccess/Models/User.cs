using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            UserByTypes = new HashSet<UserByType>();
        }

        public int Id { get; set; }
        public int UserInformationId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; }

        public virtual UserInformation UserInformation { get; set; }
        public virtual ICollection<UserByType> UserByTypes { get; set; }
    }
}
