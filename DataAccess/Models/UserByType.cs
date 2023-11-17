using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class UserByType
    {
        public UserByType()
        {
            CourseBySemesterEnrollUserByTypeStudents = new HashSet<CourseBySemesterEnroll>();
            CourseBySemesterEnrollUserByTypeTeachers = new HashSet<CourseBySemesterEnroll>();
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserTypeId { get; set; }

        public virtual User User { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual ICollection<CourseBySemesterEnroll> CourseBySemesterEnrollUserByTypeStudents { get; set; }
        public virtual ICollection<CourseBySemesterEnroll> CourseBySemesterEnrollUserByTypeTeachers { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}