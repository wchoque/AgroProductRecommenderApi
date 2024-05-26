using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Controllers.Admin;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _dbContext;

        public AdminController(AgroProductRecommenderDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("update-requests")]
        public async Task<IActionResult> GetUpdateRequests()
        {
            var groupedRequests = await _dbContext.UpdateRequests
                .Include(r => r.User)
                .ThenInclude(u => u.UserByTypes)
                .Include(r => r.User)
                .ThenInclude(u => u.UserInformation)
                .Where(x => x.Status == UpdateRequestStatus.Pending)
                .GroupBy(r => r.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    UserName = g.First().User.UserName,
                    UserType = g.First().User.UserByTypes.First().UserType.Name, // Assuming there's a `Name` property in `UserType`
                    RequestDate = g.Max(r => r.RequestedAt), // Latest request date
                    ChangeCount = g.GroupBy(r => r.FieldName).Count(), // Unique fields count
                    FirstName = g.First().User.UserInformation.FirstName,
                    LastName = g.First().User.UserInformation.LastName
                })
                .ToListAsync();

            var response = groupedRequests.Select(gr => new
            {
                UserId = gr.UserId,
                UserName = $"{gr.FirstName} {gr.LastName}",
                UserType = gr.UserType,
                RequestDate = gr.RequestDate.ToString("dd/MM/yyyy"),
                ChangeCount = gr.ChangeCount
            });

            return Ok(response);
        }

        [HttpGet("update-requests/users/{userId}")]
        public async Task<IActionResult> GetUserUpdateRequests(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserInformation)
                .Include(u => u.UserByTypes)
                .ThenInclude(ut => ut.UserType)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var updateRequests = await _dbContext.UpdateRequests
                             .Where(r => r.UserId == userId && r.Status == UpdateRequestStatus.Pending)
                             .GroupBy(r => r.FieldName)
                             .Select(g => g.OrderByDescending(r => r.RequestedAt).First())
                             .ToListAsync();

            var updateRequestsResponse = updateRequests.Select(r => new UserChangeRequest
            {
                Field = r.FieldName,
                OldValue = r.OldValue,
                NewValue = r.NewValue,
            }).ToList();

            return Ok(updateRequestsResponse);
        }


        [HttpPost("update-requests/users/{userId}/approve")]
        public async Task<IActionResult> ApproveUpdateRequest(int userId)
        {
            var requests = await _dbContext.UpdateRequests
                .Where(x => x.UserId == userId && x.Status == UpdateRequestStatus.Pending)
                .ToListAsync();

            if (!requests.Any())
            {
                return NotFound();
            }

            foreach (var request in requests)
            {
                request.Status = UpdateRequestStatus.Approved;
                request.ReviewedAt = DateTime.Now;
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.AccountStatus = UserAccountStatus.Approved;
            }

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("update-requests/users/{userId}/reject")]
        public async Task<IActionResult> RejectUpdateRequest(int userId, [FromBody] RejectChangeModel model)
        {
            var requests = await _dbContext.UpdateRequests
                .Where(x => x.UserId == userId && x.Status == UpdateRequestStatus.Pending)
                .ToListAsync();

            if (!requests.Any())
            {
                return NotFound();
            }

            foreach (var request in requests)
            {
                request.Status = UpdateRequestStatus.Rejected;
                request.AdminComment = model.Comment;
                request.ReviewedAt = DateTime.Now;
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.AccountStatus = UserAccountStatus.Rejected;
            }

            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}