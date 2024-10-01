using System;
using System.ComponentModel.DataAnnotations;

namespace frontend.Models;

public class EventRequest
{
    public string OrganizerId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event name is required!")]
    [StringLength(50)]
    public string EventName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event description is required!")]
    [StringLength(50)]
    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Event location is required!")]
    [StringLength(50)]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Select event category")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a event category")]
    public int CategoryId { get; set; }

    public List<Ticket> Tickets { get; set; } = [];
}
