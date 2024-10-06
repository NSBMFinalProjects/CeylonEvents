using EventHandler.Data;
using EventHandler.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EventDbContext _context;

        public RequestController(UserManager<AppUser> userManager,EventDbContext eventDbContext)
        {
            _userManager = userManager;
            _context = eventDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRequest(string reqDetails)
        {
            var user = await _userManager.GetUserAsync(User);  // Get the current logged-in user
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var request = new Requests
            {
                reqDetails = reqDetails,
                userId = user.Id,
                AppUser = user
            };

            _context.requests.Add(request);
            await _context.SaveChangesAsync();  

            return Ok("Request submitted successfully.");
        }
    }
}
