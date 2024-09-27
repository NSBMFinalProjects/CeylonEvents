    using EventHandler.Dto;

namespace EventHandler.Services.EventService
{
    public interface IEventService
    {
        Task<Event> GetEventDetails(int eventId);

        Task<List<Event>> GetAllEventsAsync();

        
    }
}
