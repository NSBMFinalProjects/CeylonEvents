using System;

namespace frontend.Models;

public class EventResponce
{
    public int EventId { get; set;} 
    public string EventName { get; set; }= string.Empty;
    public DateTime EventDate   { get; set; }
    public string EventDescription  { get; set; } = string.Empty;
    public string EventLocation { get; set; } = string.Empty;
    public int EventTicketPrice { get; set; }
    public string EventCategory { get; set; } = string.Empty;
    public bool IsVerifiedEvent { get; set; }
    public string EventImgUrl { get; set; } = string.Empty;
    public Stream? CardImage { get; set; }
}
