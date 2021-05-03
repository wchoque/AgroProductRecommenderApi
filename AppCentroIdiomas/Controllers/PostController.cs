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
    public class PostController : ControllerBase
    {
        private readonly AppCentroEstudiosDBContext _context;

        public PostController(AppCentroEstudiosDBContext context)
        {
            _context = context;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostModel>>> GetPosts()
        {
            //return await _context.Posts.ToListAsync();
            var _posts = new List<PostModel>();
            var posts = await _context.Posts
                .Include(x => x.UserByType)
                .Include(x => x.UserByType.User)
                .Include(x => x.UserByType.User.UserInformation)
                .ToListAsync();

            foreach (var post in posts)
            {
                var _post = new PostModel {
                    Id = post.Id,
                    Name = post.Name,
                    Description = post.Description,
                    ImageUrl = post.ImageUrl,
                    PublishedAt = post.PublishedAt.ToString("yyyy/MM/dd HH:mm:ss"),
                    PostedBy = string.Concat(post.UserByType.User.UserInformation.FirstName, " ", post.UserByType.User.UserInformation.LastName)
                };
                _posts.Add(_post);
            }
            return _posts;
        }


        // GET: api/Post/5
        [HttpGet("GetPostsByName/{name}")]
        public ActionResult<IEnumerable<PostModel>> GetPostsByName(string name)
        {
            //var post = await _context.Posts.FindAsync(id);

            //if (post == null)
            //{
            //    return NotFound();
            //}

            //return post;
            var _postModel = new List<PostModel>();
            var posts = _context.Posts
                .Include(x => x.UserByType)
                .Include(x => x.UserByType.User)
                .Include(x => x.UserByType.User.UserInformation)
                .Where(x => x.Name.Contains(name))
                .ToList();

            if (posts == null)
            {
                return NotFound();
            }

            foreach (var item in posts)
            {
                var _post = new PostModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    ImageUrl = item.ImageUrl,
                    PublishedAt = item.PublishedAt.ToString("yyyy/MM/dd HH:mm:ss"),
                    PostedBy = string.Concat(item.UserByType.User.UserInformation.FirstName, " ", item.UserByType.User.UserInformation.LastName)
                };
                _postModel.Add(_post);
            }

            return _postModel;
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostModel>> GetPost(int id)
        {
            //var post = await _context.Posts.FindAsync(id);

            //if (post == null)
            //{
            //    return NotFound();
            //}

            //return post;
            var post = await _context.Posts
                .Include(x => x.UserByType)
                .Include(x => x.UserByType.User)
                .Include(x => x.UserByType.User.UserInformation)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var _post = new PostModel {
                Id = post.Id,
                Name = post.Name,
                Description = post.Description,
                ImageUrl = post.ImageUrl,
                PublishedAt = post.PublishedAt.ToString("yyyy/MM/dd HH:mm:ss"),
                PostedBy = string.Concat(post.UserByType.User.UserInformation.FirstName, " ", post.UserByType.User.UserInformation.LastName)
            };
            return _post;
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/Post
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
