using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class CourseBySemesterEvaluation
    {
        public CourseBySemesterEvaluation()
        {
            Notes = new HashSet<Note>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int WeightPercentage { get; set; }
        public int CourseBySemesterId { get; set; }

        public virtual CourseBySemester CourseBySemester { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}