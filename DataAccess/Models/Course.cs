using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseBySemesters = new HashSet<CourseBySemester>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CourseBySemester> CourseBySemesters { get; set; }
    }
}