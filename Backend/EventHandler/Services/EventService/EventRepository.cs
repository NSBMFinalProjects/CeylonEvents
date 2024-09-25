
using EventHandler.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EventHandler.Services.EventService
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _context;
        public EventRepository(EventDbContext context)
        {
            _context = context;
        }
        public Task AddEventAsync(Event eventEntity)
        {
            _context.Events.Add(eventEntity);
            return _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int eventId)
        {
            var eventEnetity=await GetEventByIdAsync(eventId);
            if (eventEnetity != null)
            {
                _context.Events.Remove(eventEnetity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.category)
                .Include(e => e.tickets)
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int eventId)
        {
            return await _context.Events
                .Include(e => e.category)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task UpdateEventAsync(Event eventEntity)
        {
            _context.Events.Update(eventEntity);
            await _context.SaveChangesAsync();
        }
    }
}
