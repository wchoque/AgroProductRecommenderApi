using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class CourseBySemesterEnroll
    {
        public CourseBySemesterEnroll()
        {
            Notes = new HashSet<Note>();
        }

        public int Id { get; set; }
        public int CourseBySemesterId { get; set; }
        public int UserByTypeStudentId { get; set; }
        public int UserByTypeTeacherId { get; set; }

        public virtual CourseBySemester CourseBySemester { get; set; }
        public virtual UserByType UserByTypeStudent { get; set; }
        public virtual UserByType UserByTypeTeacher { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
