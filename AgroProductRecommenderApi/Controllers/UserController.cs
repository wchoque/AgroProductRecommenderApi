using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Controllers.DTOs;
using AgroProductRecommenderApi.Models;
using AgroProductRecommenderApi.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public UserController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

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

            _dbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
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
            var user = _dbContext.Users
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            var userInformation = _dbContext.UserInformation.FirstOrDefault(x => x.Id == user.UserInformationId);

            if (userInformation == null)
                return NotFound();


            var updateRequests = ValidateUpdateRequest(updatedInfo, userInformation, user).ToList();
            if (updateRequests.Any())
            {
                await _dbContext.UpdateRequests.AddRangeAsync(updateRequests);
                user.AccountStatus = UserAccountStatus.PendingApproval;
            }

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

            await _dbContext.SaveChangesAsync();

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

        private static IEnumerable<UpdateRequest> ValidateUpdateRequest2(UserInformationModel updatedInfo, UserInformation userInformation, User user)
        {
            var updateRequests = new List<UpdateRequest>();

            if (userInformation.FirstName != updatedInfo.FirstName)
            {
                updateRequests.Add(new UpdateRequest
                {
                    UserId = user.Id,
                    FieldName = nameof(UserInformation.FirstName),
                    OldValue = user.UserInformation.FirstName,
                    NewValue = updatedInfo.FirstName,
                    Status = UpdateRequestStatus.Pending,
                    RequestedAt = DateTime.Now
                });
            }

            if (userInformation.LastName != updatedInfo.LastName)
            {
                updateRequests.Add(new UpdateRequest
                {
                    UserId = user.Id,
                    FieldName = nameof(UserInformation.LastName),
                    OldValue = user.UserInformation.LastName,
                    NewValue = updatedInfo.LastName,
                    Status = UpdateRequestStatus.Pending,
                    RequestedAt = DateTime.Now
                });
            }

            return updateRequests;
        }

        private static readonly HashSet<string> WhiteListedProperties = new HashSet<string>
        {
            nameof(UserInformation.FirstName),
            nameof(UserInformation.LastName),
            nameof(UserInformation.Email),
            nameof(UserInformation.PhoneNumber),
            nameof(UserInformation.Gender),
            nameof(UserInformation.Bio),
            nameof(UserInformation.WebpageUrl),
            nameof(UserInformation.Dni)
            //nameof(UserInformation.ImageUrl)
            //nameof(UserInformation.AvatarData)
        };

        private static IEnumerable<UpdateRequest> ValidateUpdateRequest(UserInformationModel updatedInfo, UserInformation userInformation, User user)
        {
            var updateRequests = new List<UpdateRequest>();
            var userInformationProperties = typeof(UserInformation).GetProperties();
            var updatedInfoProperties = typeof(UserInformationModel).GetProperties();

            foreach (var property in userInformationProperties)
            {
                if (!WhiteListedProperties.Contains(property.Name))
                {
                    continue;
                }

                var userInformationValue = property.GetValue(userInformation)?.ToString();
                var updatedInfoProperty = updatedInfoProperties.First(p => p.Name == property.Name);
                var updatedInfoValue = updatedInfoProperty.GetValue(updatedInfo)?.ToString();

                if (userInformationValue != updatedInfoValue)
                {
                    updateRequests.Add(new UpdateRequest
                    {
                        UserId = user.Id,
                        FieldName = property.Name,
                        OldValue = userInformationValue,
                        NewValue = updatedInfoValue,
                        Status = UpdateRequestStatus.Pending,
                        RequestedAt = DateTime.Now
                    });
                }
            }

            //// Special case for byte arrays (AvatarData)
            //if (WhiteListedProperties.Contains(nameof(UserInformation.AvatarData)) &&
            //    !userInformation.AvatarData.SequenceEqual(updatedInfo.AvatarData))
            //{
            //    updateRequests.Add(new UpdateRequest
            //    {
            //        UserId = user.Id,
            //        FieldName = nameof(UserInformation.AvatarData),
            //        OldValue = Convert.ToBase64String(userInformation.AvatarData),
            //        NewValue = Convert.ToBase64String(updatedInfo.AvatarData),
            //        Status = UpdateRequestStatus.Pending,
            //        RequestedAt = DateTime.Now
            //    });
            //}

            return updateRequests;
        }


        [HttpGet("users/{id}/profile-picture")]
        public IActionResult GetProfilePicture(int id)
        {

            var user = _dbContext.Users
                .FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            var userInformation = _dbContext.UserInformation.FirstOrDefault(x => x.Id == user.UserInformationId);
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
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel createUserModel)
        {
            var usernameExists = await _dbContext.Users.AnyAsync(x => x.UserName == createUserModel.UserName);
            if (usernameExists)
            {
                return BadRequest("Username already exists.");
            }

            var userInformation = new UserInformation
            {
                FirstName = createUserModel.UserInformation.FirstName,
                LastName = createUserModel.UserInformation.LastName,
                Email = createUserModel.UserInformation.Email,
                //PhoneNumber = createUserModel.UserInformation.PhoneNumber,
                //Gender = createUserModel.UserInformation.Gender,
                //Bio = createUserModel.UserInformation.Bio,
                //WebpageUrl = createUserModel.UserInformation.WebpageUrl,
                //Dni = createUserModel.UserInformation.Dni
            };

            var user = new User
            {
                UserName = createUserModel.UserName,
                Password = PasswordHasher.HashPassword(createUserModel.Password),
                AvatarUrl = createUserModel.AvatarUrl,
                IsActive = true,
                UserInformation = userInformation,
                AccountStatus = UserAccountStatus.Created
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var userByType = new UserByType
            {
                UserId = user.Id,
                UserTypeId = createUserModel.UserTypeId
            };

            _dbContext.UserByTypes.Add(userByType);
            await _dbContext.SaveChangesAsync();

            return Ok(new { UserId = user.Id });
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _dbContext.Users.Any(e => e.Id == id);
        }
    }
}