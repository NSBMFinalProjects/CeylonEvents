using EventHandler.Data;
using EventHandler.Dto;
using EventHandler.Models.Entities;
using EventHandler.Services.EventService;
using EventHandler.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cmp;

namespace EventHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly EventDbContext _context;
        private readonly IEventService _eventService;
        private readonly RoleService _roleService;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(EventDbContext eventDbContext,IEventService eventService, RoleService roleService, UserManager<AppUser> userManager)
        {
            _context = eventDbContext;
            _eventService = eventService;
            _roleService = roleService;
            _userManager = userManager;
        }


        //get the pending and approved events
        [HttpGet("EventType")]
       
        public async Task<IActionResult> GetApprovedAndPendingEvents()
        {

            var eventsApproved = await _context.Events
                .Include(e => e.tickets)
                .Where(e => e.EventType == "Approved")
                .ToListAsync() ?? new List<Event>();

            var eventsPending = await _context.Events
                    .Include(e => e.tickets)
                    .Where(e => e.EventType == "Pending")
                    .ToListAsync() ?? new List<Event>();

            var eventDtosApproved = eventsApproved.Select(e => new EventCategoryDto
            {
                EventId = e.Id,
                EventName = e.EventName,
                EventDate = e.StartDate.ToString("yyyy-MM-dd"),
                EventTime = e.StartDate.ToString("h:mm tt"),
                EventLocation = e.Location,
                EventTicketPrice = e.tickets != null && e.tickets.Any() ? $"{e.tickets.First().Price}" : "No tickets",
                EventDescription = e.Description,
                EventImage = e.Image != null
                            ? $"{Request.Scheme}://{Request.Host}/Uploads/{e.Image}"
                            : null,
            }).ToList();

            var eventDtosPending = eventsPending.Select(e => new EventCategoryDto
            {
                EventId = e.Id,
                EventName = e.EventName,
                EventDate = e.StartDate.ToString("yyyy-MM-dd"),
                EventTime = e.StartDate.ToString("h:mm tt"),
                EventLocation = e.Location,
                EventTicketPrice = e.tickets != null && e.tickets.Any() ? $"{e.tickets.First().Price}" : "No tickets",
                EventDescription = e.Description,
                EventImage = e.Image != null
                            ? $"{Request.Scheme}://{Request.Host}/Uploads/{e.Image}"
                            : null,
            }).ToList();

            var Result = new
            {
                ApprovedEvents = eventDtosApproved,
                PendingEvents = eventDtosPending
            };

            return Ok(Result);
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsersInSystem()
        {
            var users = await _context.Users.ToListAsync();

            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NIC = user.NIC,
                PhoneNumber = user.PhoneNumber
            });

            return Ok(userDtos);
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetAllEventsInSystem()
        {
            var events = await _context.Events
                       .Include(e => e.tickets)
                       .Include(e => e.category)
                       .ToListAsync();

            var eventDtos = events.Select(eventEntity => new EventDto
            {
                EventId = eventEntity.Id,
                EventName = eventEntity.EventName,
                EventDate = eventEntity.StartDate.ToString("yyyy-MM-dd"),
                EventTime = eventEntity.StartDate.ToShortTimeString(),
                EventLocation = eventEntity.Location,
                EventTicketPrice = eventEntity.tickets?.FirstOrDefault()?.Price.ToString("c") ?? "no tickets",
                EventDescription = eventEntity.Description,
                EventCategory = eventEntity.category.Name,
                EventImage = eventEntity.Image != null
                            ? $"{Request.Scheme}://{Request.Host}/Uploads/{eventEntity.Image}"
                            : null,
            });

            return Ok(eventDtos);

        }

        [HttpGet("Organisers")]
        public async Task<IActionResult> GetAllOrganisersInSystem()
        {
            var organiserRoleId = "3";

            var organiserUserIds = await _context.UserRoles
                .Where(ur => ur.RoleId == organiserRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var organisers = await _context.Users
                .Where(u => organiserUserIds.Contains(u.Id))
                .ToListAsync();

            var userDtos = organisers.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NIC = user.NIC,
                PhoneNumber = user.PhoneNumber
            });

            return Ok(userDtos);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventEntity = await _eventService.GetEventDetails(id);

            if (eventEntity == null || eventEntity.EventType != "Pending")
            {
                return NotFound();
            }

            var imageUrl = $"{Request.Scheme}://{Request.Host}/Uploads/{eventEntity.Image}";
            var eventDto = new EventDto
            {

                EventId = eventEntity.Id,
                EventName = eventEntity.EventName,
                EventDate = eventEntity.StartDate.ToString("yyyy-MM-dd"),
                EventTime = eventEntity.StartDate.ToShortTimeString(),
                EventLocation = eventEntity.Location,
                EventTicketPrice = eventEntity.tickets?.FirstOrDefault()?.Price.ToString("c") ?? "no tickets",
                EventDescription = eventEntity.Description,
                EventCategory = eventEntity.category.Name,
                EventImage = imageUrl,
            };

            return Ok(eventDto);
        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApprovePendingEvent(int id)
        {
            
            var eventEntity = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
            {
                return NotFound(new { message = "Event not found." });
            }
            
            if (eventEntity.EventType != "Pending")
            {
                return BadRequest(new { message = "Only pending events can be approved." });
            }
            
            eventEntity.EventType = "Approved";
           
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event has been approved successfully." });
        }

        [HttpPut("Reject/{id}")]
        public async Task<IActionResult> RejectPendingEvent(int id)
        {

            var eventEntity = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            if (eventEntity.EventType != "Pending")
            {
                return BadRequest(new { message = "Only pending events can be Rejected." });
            }

            eventEntity.EventType = "Reject";

            await _context.SaveChangesAsync();

            return Ok(new { message = "Event has been Rejected successfully." });
        }

        [HttpGet("requests")]
        public async Task<IActionResult> ViewRequests()
        {
            var requests = await _context.requests
                            .Include(r => r.AppUser)
                            .ToListAsync();
            var requestsOfUsers = requests.Select(request=> new RequestDto
            {
                Id=request.ReqId,
                ReqDetails=request.reqDetails,
                UserId=request.userId,
                UserName =request.AppUser?.FirstName ?? "no user name",

            });
            return Ok(requestsOfUsers);
        }

        [HttpPost("AssignOrganizer")]
        public async Task<IActionResult> AssignOrganizerRole(int requestId)
        {
            var request = await _context.requests.Include(r => r.AppUser).FirstOrDefaultAsync(r => r.ReqId == requestId);
            if (request == null)
            {
                return NotFound("Request not found.");
            }

            var user = request.AppUser;

            var result = await _roleService.AssignOrganizerRole(user);
            if (result.Succeeded)
            {
                _context.requests.Remove(request);  // Remove the request after assigning the role
                await _context.SaveChangesAsync();

                return Ok("Organizer role assigned successfully.");
            }

            return BadRequest("Error assigning organizer role.");
        }





    }
}
