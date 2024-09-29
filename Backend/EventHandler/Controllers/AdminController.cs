using EventHandler.Data;
using EventHandler.Dto;
using EventHandler.Services.EventService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly EventDbContext _context;
        private readonly IEventService _eventService;

        public AdminController(EventDbContext eventDbContext,IEventService eventService)
        {
            _context = eventDbContext;
            _eventService = eventService;
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
                EventImage = e.Image,
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
                EventImage = e.Image,
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



    }
}
