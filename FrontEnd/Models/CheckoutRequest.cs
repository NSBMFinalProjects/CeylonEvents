using System;

namespace frontend.Models;

public class CheckoutRequest
{
    public int TiketId { get; set; }
    public int EventId { get; set; }
    public string UserID { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TicketPrice { get; set; }
    public int Quantity { get; set; }
}
