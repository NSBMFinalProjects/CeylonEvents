﻿
using EventHandler.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EventHandler.Services.EventService
{
    public class Eventservice : IEventService
    {
        private readonly IEventRepository _eventrepository;

        public Eventservice(IEventRepository eventRepository)
        {
            _eventrepository = eventRepository;
        }
        public async Task<Event> GetEventDetails(int eventId)
        {
            return await _eventrepository.GetEventByIdAsync(eventId);
        }
        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _eventrepository.GetAllEventsAsync();
        }

        
        
    }
}
