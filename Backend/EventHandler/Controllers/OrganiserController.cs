using System.Security.Claims;
using EventHandler.Data;
using EventHandler.Dto;
using EventHandler.Helper;
using EventHandler.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EventHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Organiser")]
    public class OrganiserController : ControllerBase
    {
        private readonly EventDbContext _context;
        private readonly UploadHandler _uploadHandler;
        private readonly UserManager<AppUser> _userManager;

        public OrganiserController(EventDbContext eventDbContext, UploadHandler uploadHandler, UserManager<AppUser> userManager)
        {
            _context = eventDbContext;
            _uploadHandler = uploadHandler;
            _userManager = userManager;
        }

        [HttpPost("createEvent")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateEvent([FromForm] EventWithTicketsDto eventDto, [FromForm] string tickets, [FromForm] IFormFile imageFile)
        {
            //cheak for validation
            if (eventDto == null || imageFile == null)
            {
                return BadRequest(new { Message = "Invalid user data or file" });
            }

            if (string.IsNullOrWhiteSpace(eventDto.EventName))
            {
                return BadRequest("Event name is required.");
            }

            if (eventDto.StartDate == DateTime.MinValue || eventDto.EndDate == DateTime.MinValue)
            {
                return BadRequest("Start and end dates are required.");
            }

            if (eventDto.EndDate < eventDto.StartDate)
            {
                return BadRequest("End date cannot be before the start date.");
            }


            //handle File Upload
            var result = await _uploadHandler.UploadAsync(imageFile);

            if (result.StartsWith("Extension is not valid") || result.StartsWith("Maximum size"))
            {
                return BadRequest(new { Message = result });
            }

            //get current user and assign to userid
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (eventDto.OrganizerId != loggedInUserId)
            {
                return StatusCode(403, new { message = "You are not authorized to create this event." });
            }

            List<Ticket> eventTickets = null;
            if (!string.IsNullOrEmpty(tickets))
            {
                try
                {
                    eventTickets = JsonConvert.DeserializeObject<List<Ticket>>(tickets);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = "Invalid ticket format.", Details = ex.Message });
                }
            }


            var eventEntity = new Event
            {
                OrganizerId = loggedInUserId,
                EventName = eventDto.EventName,
                Description = eventDto.Description,
                StartDate = eventDto.StartDate,
                EndDate = eventDto.EndDate,
                Location = eventDto.Location,
                CategoryId = eventDto.CategoryId,
                Image = result,
                tickets = eventTickets ?? new List<Ticket>()
            };


            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event created successfully", eventId = eventEntity.Id });
        }

        [HttpGet("Organiser")]
        
        public async Task<IActionResult> GetEventByCategory()
        {
            var OrganiserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var organiser = await _context.Users
                .Include(c => c.Events)
                .FirstOrDefaultAsync(c => c.Id == OrganiserID);

            if (organiser == null)
            {
                return NotFound($"Orginser with ID {OrganiserID} not found");
            }

            var eventDtos = organiser.Events.Select(e => new EventCategoryDto
            {
                EventId = e.Id,
                EventName = e.EventName,
                EventDate = e.StartDate.ToString("yyyy-MM-dd"),
                EventTime = e.StartDate.ToShortTimeString(),
                EventLocation = e.Location,
                EventTicketPrice = e.tickets?.FirstOrDefault()?.Price.ToString("c") ?? "no tickets",
                EventDescription = e.Description,
                EventImage = e.Image != null
                            ? $"{Request.Scheme}://{Request.Host}/Uploads/{e.Image}"
                            : null,
            }).ToList();

            return Ok(eventDtos);


        }
        [HttpGet("Organiser-Details")]
        public async Task<ActionResult<UserDto>> GetUserDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(currentUserId!);

            if (user is null)
            {
                return NotFound(new AuthResponseDto
                {
                    Message = "User not found"
                });
            }



            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NIC = user.NIC,
                PhoneNumber = user.PhoneNumber,
            });
        }

        [HttpGet("Organiser-Events")]
        public async Task<IActionResult> GetUserEvents()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var events = await _context.Events
                .Where(e => e.OrganizerId == currentUserId)
                .Include(e=>e.tickets)
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
                EventImage = eventEntity.Image
            }).ToList();

            return Ok(eventDtos);
        }


        [HttpGet("Profit/{eventId}")]
        public async Task<IActionResult> GetProfitDetails(int eventId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

           
            var eventEntity = await _context.Events
                .Where(e => e.OrganizerId == currentUserId && e.Id == eventId)
                .Include(e => e.tickets)
                .Include(e => e.category)
                .FirstOrDefaultAsync();

            
            if (eventEntity == null)
            {
                return NotFound($"Event with ID {eventId} not found or you do not have permission to view it.");
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

            
            var purchases = await _context.Purchases
                .Where(p => p.EventId == eventEntity.Id)
                .ToListAsync();

            
            var totalQuantitySold = purchases.Sum(p => p.Quantity);

            
            var ticketPrice = eventEntity.tickets.FirstOrDefault()?.Price ?? 0;

            
            var totalTicketQuantity = eventEntity.tickets.FirstOrDefault()?.Quantity ?? 0;

            
            var totalRevenue = totalQuantitySold * ticketPrice;

            
            var remainingTickets = totalTicketQuantity - totalQuantitySold;

            var profitDetail = new ProfitDetailDto
            {
                TotalTikets = totalQuantitySold,
                profit = totalRevenue,
                ReminTikets = remainingTickets
            };

            return Ok(new
            {
                EventDetails = eventDto,
                ProfitDetails = profitDetail
            });
        }



    }

}
