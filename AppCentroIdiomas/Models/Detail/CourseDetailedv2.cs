using System.Collections.Generic;

namespace AppCentroIdiomas.Models.Detail
{
    public class CourseDetailedv2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; }
        public string Description { get; set; }
        public string TeacherName { get; set; }
        //horario
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
