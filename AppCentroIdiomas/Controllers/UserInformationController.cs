using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace AppCentroIdiomas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInformationController : ControllerBase
    {
        private readonly AppCentroEstudiosDBContext _context;

        public UserInformationController(AppCentroEstudiosDBContext context)
        {
            _context = context;
        }

        // GET: api/UserInformation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInformation>>> GetUserInformations()
        {
            return await _context.UserInformations.ToListAsync();
        }

        // GET: api/UserInformation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInformation>> GetUserInformation(int id)
        {
            var userInformation = await _context.UserInformations.FindAsync(id);

            if (userInformation == null)
            {
                return NotFound();
            }

            return userInformation;
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
            _context.UserInformations.Add(userInformation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInformation", new { id = userInformation.Id }, userInformation);
        }

        // DELETE: api/UserInformation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInformation>> DeleteUserInformation(int id)
        {
            var userInformation = await _context.UserInformations.FindAsync(id);
            if (userInformation == null)
            {
                return NotFound();
            }

            _context.UserInformations.Remove(userInformation);
            await _context.SaveChangesAsync();

            return userInformation;
        }

        private bool UserInformationExists(int id)
        {
            return _context.UserInformations.Any(e => e.Id == id);
        }
    }
}
