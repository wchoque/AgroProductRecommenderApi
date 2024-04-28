using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Controllers.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/user-information")]
    [ApiController]
    public class UserInformationController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _context;

        public UserInformationController(AgroProductRecommenderDBContext context)
        {
            _context = context;
        }

        // GET: api/UserInformation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInformationDTO>>> GetUserInformations()
        {
            var usersInformation = await _context.UserInformation.ToListAsync();
            var items = usersInformation.Select(x => Convert(x, "testing")).ToList();
            return items;
        }

        // GET: api/UserInformation/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserInformation(int id)
        {
            var userInformation = await _context.UserInformation.FindAsync(id);

            if (userInformation == null)
            {
                return NotFound();
            }
            var imageUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("GetProfilePicture", "User", new { id = id })}";

            return Ok(Convert(userInformation, imageUrl));
        }

        // PUT: api/UserInformation/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInformation(int id, UserInformation userInformation)
        {
            if (id != userInformation.Id)
            {
                return BadRequest();
            }

            _context.Entry(userInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInformationExists(id))
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

        // POST: api/UserInformation
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserInformation>> PostUserInformation(UserInformation userInformation)
        {
            _context.UserInformation.Add(userInformation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInformation", new { id = userInformation.Id }, userInformation);
        }

        // DELETE: api/UserInformation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInformation>> DeleteUserInformation(int id)
        {
            var userInformation = await _context.UserInformation.FindAsync(id);
            if (userInformation == null)
            {
                return NotFound();
            }

            _context.UserInformation.Remove(userInformation);
            await _context.SaveChangesAsync();

            return userInformation;
        }

        private bool UserInformationExists(int id)
        {
            return _context.UserInformation.Any(e => e.Id == id);
        }

        private UserInformationDTO Convert(UserInformation userInformation, string imageUrl)
        {
            return new UserInformationDTO
            {
                Id = userInformation.Id,
                FirstName = userInformation.FirstName,
                LastName = userInformation.LastName,
                Email = userInformation.Email,
                PhoneNumber = userInformation.PhoneNumber,
                Gender = userInformation.Gender,
                Bio = userInformation.Bio,
                WebpageUrl = userInformation.WebpageUrl,
                Dni = userInformation.Dni,
                ImageUrl = imageUrl
            };
        }
    }
}