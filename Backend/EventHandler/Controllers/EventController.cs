using System;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventHandler.Data;
using EventHandler.Dto;
using EventHandler.Helper;
using EventHandler.Models.Entities;
using EventHandler.Services.EventService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IEventRepository _eventRepository;
        private readonly EventDbContext _context;
        private readonly UploadHandler _uploadHandler;


        public EventController(IEventService eventService, IEventRepository eventRepository, EventDbContext context,UploadHandler uploadHandler)
        {
            _eventRepository = eventRepository;
            _eventService = eventService;
            _context= context;
            _uploadHandler = uploadHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventEntity = await _eventService.GetEventDetails(id);

            if (eventEntity == null || eventEntity.EventType != "Approved")
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
                EventTicketPrice = ((int)eventEntity.tickets?.FirstOrDefault()?.Price),
                EventDescription = eventEntity.Description,
                EventCategory = eventEntity.category.Name,
                EventImage= imageUrl,
            };

            return Ok(eventDto);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEvents()
        {
            var eventEntites = await _eventService.GetAllEventsAsync();

            var approvedEvents = eventEntites
                                .Where(eventEntity => eventEntity.EventType == "Approved") 
                                .ToList();

            if (eventEntites == null || !approvedEvents.Any())
            {
                return NotFound(" No approved events found");
            }

            var eventDtos = approvedEvents.Select(eventEntity => new EventDto
            {

                EventId = eventEntity.Id,
                EventName = eventEntity.EventName,
                EventDate = eventEntity.StartDate.ToString("yyyy-MM-dd"),
                EventTime = eventEntity.StartDate.ToShortTimeString(),
                EventLocation = eventEntity.Location,
                EventTicketPrice = ((int)eventEntity.tickets?.FirstOrDefault()?.Price),
                EventDescription = eventEntity.Description,
                EventCategory = eventEntity.category.Name,
                EventImage = eventEntity.Image != null
                            ? $"{Request.Scheme}://{Request.Host}/Uploads/{eventEntity.Image}"
                            : null,

            }).ToList();

            return Ok(eventDtos);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetEventByCategory([FromRoute] int categoryId)
        {
            var category = await _context.Categorys
                .Include(c => c.Events) // One to many
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                return NotFound($"Category with ID {categoryId} not found");
            }



            var eventDtos = category.Events.Select(e => new EventCategoryDto
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

        




    }


}
