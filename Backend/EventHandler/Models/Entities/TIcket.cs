using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ticket
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Event")]
    public int EventId { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public string TicketName { get; set; }

    [Required]
    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime PurchaseDate { get; set; }

    public string Status { get; set; }

    public Event Event { get; set; }

    public User User { get; set; }
}
