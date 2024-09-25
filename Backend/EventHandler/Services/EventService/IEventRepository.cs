namespace EventHandler.Services.EventService
{
    public interface IEventRepository
    {
        Task<Event> GetEventByIdAsync(int eventId);
        
        Task<List<Event>> GetAllEventsAsync();

        Task AddEventAsync(Event eventEntity);
        Task UpdateEventAsync(Event eventEntity);
        Task DeleteEventAsync(int eventId);

    }
}
