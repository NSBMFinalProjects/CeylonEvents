public class EventRequestModel
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public int CategoryId { get; set; }  // CategoryId to relate to the category
    public string OrganizerId { get; set; } // OrganizerId to relate to the organizer
    public List<TicketRequestModel> Tickets { get; set; }
}

public class TicketRequestModel
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string TicketType { get; set; }
}