using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Controllers.DTOs;
using AgroProductRecommenderApi.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _context;

        public UserController(AgroProductRecommenderDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [HttpPost("{id}/update-profile"), DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateProfile(int id, [FromForm] UserInformationModel updatedInfo)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            var userInformation = _context.UserInformation.FirstOrDefault(x => x.Id == user.UserInformationId);

            if (userInformation == null)
                return NotFound();

            userInformation.FirstName = updatedInfo.FirstName;
            userInformation.LastName = updatedInfo.LastName;
            userInformation.Email = updatedInfo.Email;
            userInformation.PhoneNumber = updatedInfo.PhoneNumber;
            userInformation.Gender = updatedInfo.Gender;
            userInformation.Bio = updatedInfo.Bio;
            userInformation.WebpageUrl = updatedInfo.WebpageUrl;
            userInformation.Dni = updatedInfo.Dni;

            // Manejar la carga del archivo
            var imageFile = updatedInfo.ProfilePicture;
            if (imageFile is { Length: > 0 })
            {
                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);

                userInformation.AvatarData = memoryStream.ToArray();
            }

            await _context.SaveChangesAsync();

            var imageUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("GetProfilePicture", "User", new { id = id })}";
            var userInformationDto = new UserInformationDTO
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

            return Ok(userInformationDto);
        }


        [HttpGet("users/{id}/profile-picture")]
        public IActionResult GetProfilePicture(int id)
        {

            var user = _context.Users
                .FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            var userInformation = _context.UserInformation.FirstOrDefault(x => x.Id == user.UserInformationId);
            if (userInformation == null)
                return NotFound();

            if (userInformation.AvatarData == null)
            {
                var defaultAvatarUrl = "https://avatar-management--avatars.us-west-2.prod.public.atl-paas.net/default-avatar.png";
                return Redirect(defaultAvatarUrl);
            }

            var imageBytes = userInformation.AvatarData;
            return File(imageBytes, "image/jpeg");
        }


        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}