using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public enum UserAccountStatus
    {
        Created,
        PendingApproval,
        Rejected,
        Approved
    }

    public partial class User
    {
        public User()
        {
            UserByTypes = new HashSet<UserByType>();
            //FavoriteProducts = new HashSet<FavoriteProduct>();
        }

        public int Id { get; set; }
        public int UserInformationId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; }
        public UserAccountStatus AccountStatus { get; set; }

        public virtual UserInformation UserInformation { get; set; }
        public virtual ICollection<UserByType> UserByTypes { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        //public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; }
    }
}