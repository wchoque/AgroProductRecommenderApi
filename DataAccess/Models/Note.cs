#nullable disable

namespace DataAccess.Models
{
    public partial class Note
    {
        public int Id { get; set; }
        public decimal? Value { get; set; }
        public int CourseBySemesterEvaluationId { get; set; }
        public int CourseBySemesterEnrollId { get; set; }

        public virtual CourseBySemesterEnroll CourseBySemesterEnroll { get; set; }
        public virtual CourseBySemesterEvaluation CourseBySemesterEvaluation { get; set; }
    }
}