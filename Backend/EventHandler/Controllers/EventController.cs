using EventHandler.Dto;
using EventHandler.Services.EventService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
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
    }
}
