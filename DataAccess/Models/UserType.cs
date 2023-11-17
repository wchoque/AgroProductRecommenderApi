using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class UserType
    {
        public UserType()
        {
            UserByTypes = new HashSet<UserByType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserByType> UserByTypes { get; set; }
    }
}