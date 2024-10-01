using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHandler.Models.Entities;

public class Ticket
{
    
    public int Id { get; set; }
    
    public int EventId { get; set; }

    public string? UserId { get; set; }

    public string TicketName { get; set; }

    [Required]
    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public string Status { get; set; } = "Available";

    public List<Purchase> Purchases { get; set; }

    public Event Event { get; set; }

    public AppUser? AppUser { get; set; }

    public Ticket()
    {
        Purchases = new List<Purchase>(); // Initialize the collection to avoid null reference issues
    }
}
