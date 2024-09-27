using System;

namespace frontend.Models;

public class Event
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string EventDate { get; set; } = string.Empty;
    public string EventTime { get; set; } = string.Empty;
    public bool IsEventVerified { get; set; }
    public string EventLocation { get; set; } = string.Empty;
    public string EventTicketPrice { get; set; } = string.Empty;
    public string EventDescription { get; set; } = string.Empty;
    public string EventCategory { get; set; } = string.Empty;
    public string EventCardImageUrl { get; set; } = string.Empty;
}
