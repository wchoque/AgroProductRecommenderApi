using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using AppCentroIdiomas.Models;

namespace AppCentroIdiomas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseDetailController : ControllerBase
    {
        private readonly AppCentroEstudiosDBContext _context;

        public CourseDetailController(AppCentroEstudiosDBContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDetailedModel>>> GetCourses()
        {
            var courses = await _context.Courses
                                .Include(x => x.CourseBySemesters.Select(x => x.Schedules))
                                .Include(x => x.CourseBySemesters.Select(x => x.CourseBySemesterEnrolls))
                                .ToListAsync();
            var coursesDetailed = new List<CourseDetailedModel>();
            foreach (var course in courses)
            {
                var _courseDetailed = new CourseDetailedModel {
                    Course = course,
                    Schedules = (ICollection<Schedule>)course.CourseBySemesters.Select(x => x.Schedules),
                    Students = (ICollection<UserInformation>)course.CourseBySemesters.Select(x => x.CourseBySemesterEnrolls.Select(x => x.UserByTypeStudent.User.UserInformation)),
                    Teacher = (UserInformation)course.CourseBySemesters.Select(x => x.CourseBySemesterEnrolls.Select(x => x.UserByTypeTeacher.User.UserInformation))
                };
                coursesDetailed.Add(_courseDetailed);
            }
            return coursesDetailed;
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
