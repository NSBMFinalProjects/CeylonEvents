using System;
using System.Security.Claims;
using EventHandler.Data;
using EventHandler.Dto;
using EventHandler.Services.EventService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IEventRepository _eventRepository;
        private readonly EventDbContext _context;


        public EventController(IEventService eventService, IEventRepository eventRepository, EventDbContext context)
        {
            _eventRepository = eventRepository;
            _eventService = eventService;
            _context= context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventEntity = await _eventService.GetEventDetails(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            var eventDto = new EventDto
            {
                EventId = eventEntity.Id,
                EventName = eventEntity.EventName,
                EventDate = eventEntity.StartDate.ToShortDateString(),
                EventTime = eventEntity.StartDate.ToShortTimeString(),
                EventLocation = eventEntity.Location,
                EventTicketPrice = eventEntity.tickets?.FirstOrDefault()?.Price.ToString("c") ?? "no tickets",
                EventDescription = eventEntity.Description,
                EventCategory = eventEntity.category.Name
            };

            return Ok(eventDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var eventEntites = await _eventService.GetAllEventsAsync();

            if (eventEntites == null || !eventEntites.Any())
            {
                return NotFound(" no events Found");
            }

            var eventDtos = eventEntites.Select(eventEntity => new EventDto
            {
                EventId = eventEntity.Id,
                EventName = eventEntity.EventName,
                EventDate = eventEntity.StartDate.ToShortDateString(),
                EventTime = eventEntity.StartDate.ToShortTimeString(),
                EventLocation = eventEntity.Location,
                EventTicketPrice = eventEntity.tickets?.FirstOrDefault()?.Price.ToString("c") ?? "no tickets",
                EventDescription = eventEntity.Description,
                EventCategory = eventEntity.category.Name,
            }).ToList();

            return Ok(eventDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateEvent([FromBody] EventWithTicketsDto eventDto)
        {
            
            if (eventDto == null)
            {
                return BadRequest("Event details are required.");
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

            if (eventDto.Tickets == null || !eventDto.Tickets.Any())
            {
                return BadRequest("At least one ticket must be provided.");
            }

            foreach (var ticket in eventDto.Tickets)
            {
                if (string.IsNullOrWhiteSpace(ticket.TicketName))
                {
                    return BadRequest("Ticket name is required.");
                }

                if (ticket.Quantity <= 0)
                {
                    return BadRequest("Ticket quantity must be greater than zero.");
                }

                if (ticket.Price < 0)
                {
                    return BadRequest("Ticket price cannot be negative.");
                }
            }
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            
            if (eventDto.OrganizerId != loggedInUserId)
            {
                return StatusCode(403, new { message = "You are not authorized to create this event." });
            }

            var eventEntity = new Event
            {
                OrganizerId = eventDto.OrganizerId,
                EventName = eventDto.EventName,
                Description = eventDto.Description,
                StartDate = eventDto.StartDate,
                EndDate = eventDto.EndDate,
                Location = eventDto.Location,
                CategoryId = eventDto.CategoryId,
                tickets = eventDto.Tickets.Select(t => new Ticket
                {
                    TicketName = t.TicketName,
                    Quantity = t.Quantity,
                    Price = t.Price,
                }).ToList()
            };

           
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event created successfully", eventId = eventEntity.Id });
        }



    }


}
