using System;
namespace frontend.Models;
public class Event
{
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDateTime { get; set; }
    public string EventLocation { get; set; } = string.Empty;
    public string EventTicketCount { get; set; } = string.Empty ;
    public string EventTicketPrice { get; set; } = string.Empty;
    public string EventDescription { get; set; } = string.Empty;
    public string EventCategory { get; set; } = string.Empty;
    public string EventCardImageUrl { get; set; } = string.Empty;
}
