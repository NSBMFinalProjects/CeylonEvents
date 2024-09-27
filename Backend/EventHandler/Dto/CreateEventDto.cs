public class EventWithTicketsDto
{
    public string OrganizerId { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; }
    public int CategoryId { get; set; }
    public List<TicketDto> Tickets { get; set; }
}

public class TicketDto
{
    public string TicketName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
