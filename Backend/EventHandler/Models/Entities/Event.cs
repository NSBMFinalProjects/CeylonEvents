using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Event
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int OrganizerId { get; set; }

    [Required]
    [StringLength(100)]
    public string EventName { get; set; }

    public string Slug { get; set; }

    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Location { get; set; }

    public string EventType { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<EventAttendance> Attendances { get; set; }

    public List<Notification> notifications { get; set; }

    public List<Ticket> tickets { get; set; }

    public List<Session> Sessions { get; set; }

    public User Organizer { get; set; }
}

public class Session
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string SessionName { get; set; }

    public string Slug { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Speaker { get; set; }

    [ForeignKey("Event")]
    public int EventId { get; set; }

    public Event Event { get; set; }
}
