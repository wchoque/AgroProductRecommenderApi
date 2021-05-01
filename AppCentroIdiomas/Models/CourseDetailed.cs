using System.Collections.Generic;

namespace AppCentroIdiomas.Models
{
    public class CourseDetailed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; }
        public string Description { get; set; }
        public string TeacherName { get; set; }
        public IEnumerable<ScheduleModel> Schedules { get; set; }
        public IEnumerable<NoteModel> Notes { get; set; }
    }
}
