using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class CourseBySemester
    {
        public CourseBySemester()
        {
            CourseBySemesterEnrolls = new HashSet<CourseBySemesterEnroll>();
            CourseBySemesterEvaluations = new HashSet<CourseBySemesterEvaluation>();
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public int? CourseId { get; set; }
        public int SemesterId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<CourseBySemesterEnroll> CourseBySemesterEnrolls { get; set; }
        public virtual ICollection<CourseBySemesterEvaluation> CourseBySemesterEvaluations { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}