using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCentroIdiomas.Models
{
    public class CourseDetailedModel
    {
        public Course Course { get; set; }
        public UserInformation Teacher { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<UserInformation> Students { get; set; }
    }
}
