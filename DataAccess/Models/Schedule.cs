using System;

#nullable disable

namespace DataAccess.Models
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public int CourseBySemesterId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual CourseBySemester CourseBySemester { get; set; }
    }
}