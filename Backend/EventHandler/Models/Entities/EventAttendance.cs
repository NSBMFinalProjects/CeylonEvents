using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class EventAttendance
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Event")]
    public int EventId { get; set; }

    public bool CheckedIn { get; set; }

    public DateTime? CheckedInAt { get; set; }

    public User User { get; set; }

    public Event Event { get; set; }
}
