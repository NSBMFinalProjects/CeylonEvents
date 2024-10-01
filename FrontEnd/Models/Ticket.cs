using System;
using System.ComponentModel.DataAnnotations;

namespace frontend.Models;

public class Ticket
{

    public string TicketName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public int Price { get; set; }
}

