using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Semester
    {
        public Semester()
        {
            CourseBySemesters = new HashSet<CourseBySemester>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<CourseBySemester> CourseBySemesters { get; set; }
    }
}
