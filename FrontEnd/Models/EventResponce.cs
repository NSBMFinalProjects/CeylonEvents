using System;

namespace frontend.Models;

public class EventResponce
{
    
    public int EventId { get; set;} 
    public string EventName { get; set; }= string.Empty;
    public string EventDate   { get; set; } = string.Empty;
    public string EventTime   { get; set; } = string.Empty;
    public string EventLocation { get; set; } = string.Empty;
    public string EventDescription  { get; set; } = string.Empty;
    public int EventTicketPrice { get; set; }
    public string EventCategory { get; set; } = string.Empty;
    public string EventImage { get; set; } = string.Empty;
    public int PurchesedTikets { get; set; }
}
